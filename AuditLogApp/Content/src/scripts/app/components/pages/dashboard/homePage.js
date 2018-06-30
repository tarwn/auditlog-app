import PageBase from '../pageBase';
import ViewConfigurationModel from '../../../models/viewConfigurationModel';

import CustomizationColumnModel from '../../../borrowed/auditLogDropIn/models/customizationColumnModel';
import EntryTableRow from '../../../borrowed/auditLogDropIn/components/entryTableRow';

export default {
    name: 'page-home',
    viewModel: class HomePage extends PageBase {
        constructor(params) {
            super(params);

            // selectable options
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

            // TODO: build in date navigation on top bar
            this.availableDates = [];
            this.selectedDate = ko.observable();

            // raw data
            this.view = ko.observable();
            this.auditList = ko.observable();

            // dropin data
            this.dropInColumns = ko.observable();
            this.dropInRows = ko.observable();
            this.selectedRow = ko.observable();

            this.initialize();
        }

        initialize() {
            this._services.whenReady()
                .then(() => this.loadView())
                .then(() => Promise.all([
                    this.loadEvents(),
                    this.loadClients()
                ]))
                .then(() => {
                    this.readyForDisplay(true);
                });
        }

        loadView() {
            return this._services.getDashboardView().then((rawData) => {
                this.view(new ViewConfigurationModel(rawData));

                // COPIED: dropin/models/customizationModel, ln 15
                const dropInColumns = rawData.columns
                    .sort((a, b) => a.order - b.order)
                    .map(c => new CustomizationColumnModel(c));
                this.dropInColumns(dropInColumns);
            });
        }

        loadEvents() {
            return this._services.getEvents(this.selectedClientId()).then((rawData) => {
                this.auditList(rawData);

                // COPIED: dropin/components/entryTable, ln 8
                const rows = rawData.entries.map(r => new EntryTableRow(r, this.dropInColumns()));
                this.dropInRows(rows);
            });
        }

        loadClients() {
            return this._services.getClients().then((rawData) => {
                this.allClients(rawData.map((c) => {
                    return {
                        id: c.id,
                        name: c.name
                    };
                }));
            });
        }

        // COPIED: dropin/appViewModel, ln 39
        selectRow(row) {
            this.selectedRow(row);
        }

        clearSelectedRow() {
            this.selectedRow(null);
        }

        // dispose() {
        // }
    },
    // COPIED: table matches audit table templates from dropin
    template: `
        <div class="ala-loading" data-bind="visible: !readyForDisplay()">Loading...</div>
        <div class="ala-view-edit-configurations" data-bind="if: readyForDisplay">
            <h1>Dashboard</h1>

            <div>
                <select data-bind="options: allClients, value: selectedClient, optionsText: 'name', optionsCaption: 'All Clients'"></select>
                <select data-bind="options: availableDates, value: selectedDate"></select>
            </div>

            <div>
                Chart Here
            </div>

            <div>
                <table class="aldi-audit-table">
                    
                    <thead>
                        <tr class="aldi-audit-headrow">
                            <th></th>
                            <!-- ko foreach: dropInColumns -->
                                <th><span data-bind="text: label"></span><i data-bind="attr: { class: label == 'Time' ? 'icon-sort-down' : 'icon-sort' }"></i></th>
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

                    <tr style="display: none" id="aldi-details-panel" data-bind="if: selectedRow()">
                        <td data-bind="attr: { colspan: dropInColumns().length + 1 }">
                            <div class="aldi-details-inner-panel">
                                <i class="icon-cancel" data-bind="detailsHide: '#aldi-details-panel', clearRow: clearSelectedRow.bind($data), selectedClass: 'aldi-row-selected'"></i>
                                <h3><i class="icon-ellipsis-vert"></i>Event Details</h3>
                                <div class="aldi-details-subheader">Summary</div>
                                <div class="aldi-details-summary" data-bind="with: selectedRow">
                                    <table class="aldi-details-table">
                                        <tr class="aldi-details-tablerow"><th>Action</th><td data-bind="text: action"></td></tr>
                                        <tr class="aldi-details-tablerow"><th>Time</th><td data-bind="text: display.time"></td></tr>
                                        <tr class="aldi-details-tablerow"><th></th><td data-bind="text: description"></td></tr>
                                    </table>
                                </div>
                                <div class="aldi-details-subheader">Details</div>
                                <div class="aldi-details-columns" data-bind="with: $parent.selectedRow">
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
