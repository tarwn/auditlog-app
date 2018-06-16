import PageBase from '../pageBase';
import ViewConfigurationModel from '../../../models/viewConfigurationModel';
import ViewHeaderLinkModel from '../../../models/viewHeaderLinkModel';
import ViewColumnConfigurationModel from '../../../models/viewColumnConfigurationModel';

class Tabs {
    constructor(tabs) {
        this.selectedTab = ko.observable();
        tabs.forEach((tab) => {
            this[`is${tab}Selected`] = ko.pureComputed(() => this.selectedTab() === tab);
            this[`changeTo${tab}`] = () => {
                this.selectedTab(tab);
            };
        });
    }
}

export default {
    name: 'page-view-edit',
    viewModel: class ViewEditPage extends PageBase {
        constructor(params) {
            super(params);
            this.view = ko.observable();

            this.newLink = ko.observable(ViewHeaderLinkModel.getEmpty(false));
            this.newColumn = ko.observable(ViewColumnConfigurationModel.getEmpty());

            this.tabs = new Tabs(['Layout', 'Columns', 'Security']);
            this.tabs.changeToLayout();

            this.isSaving = ko.observable(false);
            this.initialize();
        }

        initialize() {
            this.loadView().then(() => {
                this.readyForDisplay(true);
            });
        }

        addNewHeaderLink() {
            const newLink = new ViewHeaderLinkModel(ko.toJS(this.newLink()), true);
            if (newLink.isValid()) {
                this.view().addHeaderLink(newLink);
                this.newLink(ViewHeaderLinkModel.getEmpty(false));
            }
            else {
                this.newLink(newLink);
            }
        }

        removeHeaderLink(link) {
            this.view().removeHeaderLink(link);
        }

        addNewColumn() {
            const newColumn = new ViewColumnConfigurationModel(ko.toJS(this.newColumn()));
            if (newColumn.isValid()) {
                this.view().addColumn(newColumn);
                this.newColumn(ViewColumnConfigurationModel.getEmpty());
            }
            else {
                this.newColumn(newColumn);
            }
        }

        removeColumn(column) {
            this.view().removeColumn(column);
        }

        moveColumnUp(column) {
            if (column.isValid()) {
                this.view().moveColumnUp(column);
            }
        }

        moveColumnDown(column) {
            if (column.isValid()) {
                this.view().moveColumnDown(column);
            }
        }

        saveChanges() {
            this.isSaving(true);
            this.saveView().then(() => {
                this.isSaving(false);
            });
        }

        loadView() {
            return this._services.getOrCreateDefaultView().then((rawData) => {
                this.view(new ViewConfigurationModel(rawData));
            });
        }

        saveView() {
            return this._services.saveView(this.view());
        }

        resetViewKey() {
            this._services.resetViewKey(this.view().id)
                .then((newKey) => {
                    this.view().accessKey(newKey.accessKey);
                });
        }

        // dispose() {
        // }
    },
    template: `
        <div class="ala-loading" data-bind="visible: !readyForDisplay()">Loading...</div>
        <div class="ala-view-edit-configurations" data-bind="if: readyForDisplay">
            <h1>Customize Client View</h1>
            <button class="ala-button" style="margin-top: -1.5em; margin-bottom: 1.5em;" data-bind="click: saveChanges">Save Changes</button>
            <ul class="ala-view-edit-menu">
                <li><button class="ala-button-tab" data-bind="click: tabs.changeToLayout, css: { 'ala-button-tab-selected': tabs.isLayoutSelected }">Layout</button></li>
                <li><button class="ala-button-tab" data-bind="click: tabs.changeToColumns, css: { 'ala-button-tab-selected': tabs.isColumnsSelected }">Columns</button></li>
                <li><button class="ala-button-tab" data-bind="click: tabs.changeToSecurity, css: { 'ala-button-tab-selected': tabs.isSecuritySelected }">Security</button></li>
            </ul>
            <div class="ala-tab-content" data-bind="visible: tabs.isLayoutSelected">
                <h2 class="ala-screen-reader-only">Layout</h2>
                <h3>Header</h3>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1" for="txtLogo">Logo</label> 
                    <input-url params="id: 'txtLogo', value: { o: view().custom.logo }" />
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1" for="txtTitle">Title</label> 
                    <input-text params="id: 'txtTitle', value: { o: view().custom.title }" />
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1" for="txtURL">URL</label> 
                    <input-url params="id: 'txtURL', value: { o: view().custom.url }" />
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1">Links</label>
                    <table class="ala-form-table">
                        <thead>
                            <tr><th>Text</th><th>URL</th><th></th></tr>
                        </thead>
                        <tbody data-bind="foreach: view().custom.headerLinks">
                            <tr>
                                <td><input-text params="value: { o: label }, isShorter: true" /></td>
                                <td><input-url params="value: { o: url }" /></td>
                                <td><button class="ala-button-lite-red" title="Remove" data-bind="click: $parent.removeHeaderLink.bind($parent)"><i class="icon-cancel"></i></button></td>
                            </tr>
                        </tbody>
                        <tfoot data-bind="with: newLink">
                            <tr>
                                <td><input-text params="value: { o: label }, isShorter: true" /></td>
                                <td><input-url params="value: { o: url }" /></td>
                                <td><button class="ala-button-lite" title="Add Link" data-bind="attr: { disabled: !isValid() }, click: $parent.addNewHeaderLink.bind($parent)"><i class="icon-plus-circled"></i>Add</button></td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
                <br />
                <h3>Footer</h3>
                <div class="ala-form-row">
                    <label class="ala-form-label-w2" for="txtCopyright">Copyright</label> 
                    <input-text params="id: 'txtCopyright', value: { o: view().custom.copyright }" />
                </div>
            </div>
            <div class="ala-tab-content" data-bind="visible: tabs.isColumnsSelected">
                <h2 class="ala-screen-reader-only">Columns</h2>
                <table class="ala-form-table">
                    <thead>
                        <tr><th>#</th><th>Label</th><th>Fields</th><th></th></tr>
                    </thead>
                    <tbody data-bind="foreach: view().sortedColumns">
                        <tr>
                            <td data-bind="text: $index() + 1"></td>
                            <td>
                                <input-text params="value: { o: label }, isShorter: true" />
                            </td>
                            <td>
                                <select data-bind="options: $parent.view().availableFields, value: lines()[0].field">
                                </select><br/>
                                <select data-bind="options: $parent.view().availableFields, value: lines()[1].field, optionsCaption: 'Second row...'" class="ala-columns-line2">
                                </select>
                            </td>
                            <td class="ala-center">
                                <button class="ala-button-lite" title="Move Up" data-bind="click: $parent.moveColumnUp.bind($parent)"><i class="icon-up-open"></i></button>
                                <button class="ala-button-lite" title="Move Down" data-bind="click: $parent.moveColumnDown.bind($parent)"><i class="icon-down-open"></i></button>
                                <button class="ala-button-lite-red" title="Remove" data-bind="click: $parent.removeColumn.bind($parent)"><i class="icon-cancel"></i></button>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot data-bind="with: newColumn">
                        <tr>
                            <td></td>
                            <td>
                                <input-text params="value: { o: label }, isShorter: true" />
                            </td>
                            <td>
                                <select data-bind="options: $parent.view().availableFields, value: lines()[0].field">
                                </select><br/>
                                <select data-bind="options: $parent.view().availableFields, value: lines()[1].field, optionsCaption: 'Second row...'">
                                </select>
                            </td>
                            <td class="ala-center">
                                <button class="ala-button-lite" title="Add Column" data-bind="click: $parent.addNewColumn.bind($parent)"><i class="icon-plus-circled"></i>Add</button>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div class="ala-tab-content" data-bind="visible: tabs.isSecuritySelected">
                <h2 class="ala-screen-reader-only">Security</h2>
                <div class="ala-form-description">
                    The <code>View Key</code> is an access key used to display this customized view to your clients. It should
                    be treated as a specialized API key, usable to access saved Event Log data but not make changes.
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w2">View Key</label> 
                    <span class="ala-form-input-faux" data-bind="text: view().accessKey" /><br/>
                </div>
                <div class="ala-form-row">
                    <button class="ala-button" data-bind="click: resetViewKey">Generate New Key</button>
                </div>
            </div>


        </div>
        <div class="ala-view-edit-example">
            IFrame goes here: <a href="https://app.clubhouse.io/launchready/story/745/views-live-example">ch-745</a>
        </div>
    `
};
