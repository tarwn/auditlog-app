import PageBase from '../pageBase';
import ViewConfigurationModel from '../../../models/viewConfigurationModel';
import DailyEntryCount from '../../charts/models/dailyEntryCount';

import CustomizationColumnModel from '../../../borrowed/auditLogDropIn/models/customizationColumnModel';
import EntryTableRow from '../../../borrowed/auditLogDropIn/components/entryTableRow';

export default {
    name: 'page-home',
    viewModel: class HomePage extends PageBase {
        constructor(params) {
            super(params);

            // User Filter
            this.isRefreshing = ko.observable(false);

            this.allClients = ko.observable();
            this.selectedClient = ko.observable();
            this.selectedClientId = ko.pureComputed(() => {
                if (this.selectedClient() != null) {
                    return this.selectedClient().id;
                }
                else {
                    return null;
                }
            });

            this.availableDates = ko.observable();
            this.selectedDate = ko.observable();
            this.displayedDate = ko.observable();

            this.filterSummary = ko.pureComputed(() => {
                if (!this.readyForDisplay()) {
                    return 'loading...';
                }
                else if (this.isRefreshing()) {
                    return 'loading...';
                }
                else {
                    return `${this.dropInRows().length} entries available`;
                }
            });

            // raw data
            this._view = ko.observable();
            this._auditList = ko.observable();

            // dropin data
            this.dropInColumns = ko.observable();
            this.dropInRows = ko.observableArray();
            this.selectedRow = ko.observable();
            this.noRowsAvailable = ko.pureComputed(() =>
                this.dropInRows() === null ||
                this.dropInRows().length === 0);
            this.sortColumn = ko.observable();
            this.sortDirection = ko.observable();

            // chart data
            this.chartData = {
                entriesByDay: ko.pureComputed(() => {
                    if (!this.readyForDisplay() ||
                        this.displayedDate() == null) {
                        return [];
                    }

                    const { entries = [] } = this._auditList();
                    return HomePage.calculateDailyEntryCounts(entries, this.displayedDate());
                })
            };

            this.initialize();
        }

        initialize() {
            this._services.whenReady()
                .then(() => this._loadView())
                .then(() => Promise.all([
                    this._initializeDates(),
                    this._initializeSorting()
                ]))
                .then(() => Promise.all([
                    this._loadEntries(),
                    this._loadClients()
                ]))
                .then(() => {
                    this._subscribeToFilterChanges();
                    this.readyForDisplay(true);
                });
        }

        _loadView() {
            return this._services.getDashboardView().then((rawData) => {
                this._view(new ViewConfigurationModel(rawData));

                // COPIED: dropin/models/customizationModel, ln 15
                const dropInColumns = rawData.columns
                    .sort((a, b) => a.order - b.order)
                    .map(c => new CustomizationColumnModel(c));
                this.dropInColumns(dropInColumns);
            });
        }

        _initializeDates() {
            const dates = [];
            const a = [];
            for (let i = 0; i < 12; i++) {
                a.push(i);
            }
            a.map(n => moment().startOf('month').subtract(n, 'months'))
                .forEach((m) => {
                    dates.push({
                        from: `${m.format('YYYY-MM-DD')}T00:00:00Z`,
                        to: `${m.clone().add(1, 'months').add(-1, 'days').format('YYYY-MM-DD')}T00:00:00Z`,
                        display: m.format('MMM YYYY')
                    });
                });
            this.availableDates(dates);
            this.selectedDate(dates[0]);

            return Promise.resolve();
        }

        _initializeSorting() {
            /* eslint-disable no-restricted-syntax */
            for (const c of this.dropInColumns()) {
                if (c.lines[0].field === 'time') {
                    this.sortColumn(c);
                    this.sortDirection('desc');
                    break;
                }
            }
            /* eslint-enable no-restricted-syntax */

            return Promise.resolve();
        }

        _loadEntries() {
            // const start = Date.now();
            const loadingDate = this.selectedDate();
            const { from, to } = loadingDate;
            return this._services.getEntries(this.selectedClientId(), from, to).then((rawData) => {
                this._auditList(rawData);

                // COPIED: dropin/components/entryTable, ln 8
                const rows = rawData.entries.map(r => new EntryTableRow(r, this.dropInColumns()));
                this.dropInRows(rows);

                this.displayedDate(loadingDate);
            });
        }

        _refreshEntries() {
            this.isRefreshing(true);
            this._loadEntries()
                .finally(() => {
                    // hacky - this is going to apply after the binding takes effect
                    // - there will be flicker: https://app.clubhouse.io/launchready/story/789/improve-sort-on-dashboard-load
                    this._sortEntries();
                    this.isRefreshing(false);
                });
        }

        _sortEntries() {
            const sortColumnName = this.sortColumn().lines[0].field;
            const sortFunction = (a, b) => {
                return EntryTableRow.compare(sortColumnName, a, b);
            };
            const sortHigh = this.sortDirection() === 'desc' ? 1 : -1;
            const sortLow = sortHigh * -1;
            this.dropInRows.sort((a, b) => {
                return sortFunction(a, b) ? sortHigh : sortLow;
            });
        }

        _loadClients() {
            return this._services.getClients().then((rawData) => {
                this.allClients(rawData.map((c) => {
                    return {
                        from: c.id,
                        name: c.name
                    };
                }));
            });
        }

        _subscribeToFilterChanges() {
            this._subscriptions.push(this.selectedClientId.subscribe(() => {
                this._refreshEntries();
            }));
            this._subscriptions.push(this.selectedDate.subscribe(() => {
                this._refreshEntries();
            }));
        }

        applySort(col) {
            if (this.sortColumn() === col) {
                if (this.sortDirection() === 'desc') {
                    this.sortDirection('asc');
                }
                else {
                    this.sortDirection('desc');
                }
            }
            else {
                this.sortColumn(col);
                this.sortDirection('desc');
            }
            this._sortEntries();
        }

        // COPIED: dropin/appViewModel, ln 39
        selectRow(row) {
            this.selectedRow(row);
        }

        clearSelectedRow() {
            this.selectedRow(null);
        }

        static calculateDailyEntryCounts(entries, selectedDate) {
            const data = [];
            const indexedData = {};
            const from = moment(selectedDate.from).utc().clone();
            const to = moment(selectedDate.to).utc();

            const getIndex = dt => `${dt.getUTCFullYear()}${`0${dt.getUTCMonth() + 1}`.slice(-2)}${dt.getUTCDate()}`;

            while (from <= to) {
                const item = new DailyEntryCount(from.clone(), 0);
                data.push(item);
                indexedData[getIndex(from.toDate())] = item;
                from.add(1, 'days');
            }

            entries.forEach((e) => {
                const indexDate = new Date(e.time);
                indexDate.setUTCHours(0, 0, 0, 0);
                const index = getIndex(indexDate);
                if (indexedData[index]) {
                    indexedData[index].incrementCount(1);
                }
            });

            return data;
        }
    },
    // COPIED: table matches audit table templates from dropin
    template: `
        <div class="ala-loading" data-bind="visible: !readyForDisplay()">Loading...</div>
        <div class="ala-view-edit-configurations" data-bind="if: readyForDisplay">
            <h1 class="ala-dashboard-title">Dashboard</h1>

            <div class="ala-dashboard-filter">
                <div class="ala-dashboard-filter-selection">
                    <select data-bind="options: allClients, value: selectedClient, optionsText: 'name', optionsCaption: 'All Clients'"></select>
                </div>
                <div class="ala-dashboard-filter-selection">
                    <select data-bind="options: availableDates, value: selectedDate, optionsText: 'display'"></select>
                </div>
                <span class="ala-dashboard-filter-summary" data-bind="text: filterSummary"></span>
            </div>

            <div class="ala-dashboard-table">
                <div class="ala-dashboard-chart-area">
                    <daily-entry-chart params="value: { data: chartData.entriesByDay }" />
                </div>

                <table class="aldi-audit-table">
                    
                    <thead>
                        <tr class="aldi-audit-headrow">
                            <th></th>
                            <!-- ko foreach: dropInColumns -->
                                <th data-bind="click: $parent.applySort.bind($parent)" class="aldi-table-sortable"><span data-bind="text: label"></span><i data-bind="css: { 'icon-sort': $data !== $parent.sortColumn(), 'icon-sort-down': $data === $parent.sortColumn() && $parent.sortDirection() === 'desc', 'icon-sort-up': $data === $parent.sortColumn() && $parent.sortDirection() === 'asc'  }"></i></th>
                            <!-- /ko -->
                        </tr>
                    </thead>

                    <tbody data-bind="foreach: dropInRows">
                        <tr class="aldi-audit-row">
                            <td class="aldi-row-doubleheight">
                                <a href="#" title="Show Details" data-bind="detailsClick: '#aldi-details-panel', rowId: id, selectRow: $parent.selectRow.bind($parent), selectedClass: 'aldi-row-selected'"><i class="icon-plus-circled"></i></a>
                            </td>
                            <!-- ko foreach: columns -->
                                <td data-bind="html: value"></td>
                            <!-- /ko -->
                        </tr>
                    </tbody>

                    <tr data-bind="if: noRowsAvailable()">
                        <td class="ala-table-no-data" data-bind="attr: { colspan: dropInColumns().length + 1 }">No entries available</td>
                    </tr>
                    <tr style="display: none" id="aldi-details-panel" data-bind="if: selectedRow()">
                        <td data-bind="attr: { colspan: dropInColumns().length + 1 }">
                            <div class="aldi-details-inner-panel">
                                <i class="icon-cancel" data-bind="detailsHide: '#aldi-details-panel', clearRow: clearSelectedRow.bind($data), selectedClass: 'aldi-row-selected'"></i>
                                <h3><i class="icon-ellipsis-vert"></i>Entry Details</h3>
                                <div class="aldi-details-subheader">Summary</div>
                                <div class="aldi-details-summary" data-bind="with: selectedRow">
                                    <table class="aldi-details-table">
                                        <tr class="aldi-details-tablerow"><th>Action</th><td data-bind="text: action"></td></tr>
                                        <tr class="aldi-details-tablerow"><th>Time</th><td data-bind="text: display.time"></td></tr>
                                        <tr class="aldi-details-tablerow"><th></th><td data-bind="text: description"></td></tr>
                                    </table>
                                </div>
                                <div class="aldi-details-subheader">Details</div>
                                <div class="aldi-details-columns" data-bind="with: selectedRow">
                                    <!-- ko if: target != null && target.type != null -->
                                    <div class="aldi-details-column">
                                        <table class="aldi-details-table">
                                            <tr class="aldi-details-tablerow"><th>Target</th><td data-bind="text: target.type"></td></tr>
                                            <tr class="aldi-details-tablerow"><th>Id</th><td data-bind="potentialLink: { text: target.uuid, url: target.url }"></td></tr>
                                            <tr class="aldi-details-tablerow"><th>Name</th><td data-bind="potentialLink: { text: target.label, url: target.url }"></td></tr>
                                        </table>
                                    </div>
                                    <!-- /ko -->
                                    <div class="aldi-details-column">
                                        <table class="aldi-details-table">
                                            <tr class="aldi-details-tablerow"><th>By</th><td data-bind="text: actor.name"></td></tr>
                                            <tr class="aldi-details-tablerow"><th>Email</th><td data-bind="text: actor.email"></td></tr>
                                            <tr class="aldi-details-tablerow"><th>IP:</th><td data-bind="text: context.client.ipAddress"></td></tr>
                                            <!-- <tr class="aldi-details-tablerow"><th>Browser:</th><td data-bind="text: context.client.browserAgent"></td></tr>-->
                                        </table>
                                    </div>
                                    <div class="aldi-details-column">
                                        <table class="aldi-details-table">
                                            <tr class="aldi-details-tablerow" data-bind="if: context.server.serverId"><th>Server</th><td data-bind="text: context.server.serverId"></td></tr>
                                            <tr class="aldi-details-tablerow" data-bind="if: context.server.version"><th>Version</th><td data-bind="text: context.server.version"></td></tr>
                                            <tr class="aldi-details-tablerow"><th>Received</th><td><span data-bind="text: display.receivedAt"></span> <span class="aldi-text-light">(<span data-bind="text: display.receivedLatency"></span>)</span></td></tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    `
};
