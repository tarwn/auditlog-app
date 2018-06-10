import PageBase from '../pageBase';
import ViewConfigurationModel from '../../../models/viewConfigurationModel';

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

            this.newLinkLabel = ko.observable();
            this.newLinkURL = ko.observable();

            this.tabs = new Tabs(['Layout', 'Columns', 'Security']);
            this.tabs.changeToLayout();

            this.initialize();
        }

        initialize() {
            this.loadView().then(() => {
                this.readyForDisplay(true);
            });
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
            return this._services.saveView(this.view);
        }

        resetViewKey() {
            this._services.resetViewKey(this.view().id)
                .then((newKey) => {
                    this.view().key(newKey);
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
                    <input-url params="id: 'txtLogo', value: view().custom.logo, maxLength: 80" />
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1" for="txtTitle">Title</label> 
                    <input-text params="id: 'txtTitle', value: view().custom.title, maxLength: 240" />
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1" for="txtURL">URL</label> 
                    <input-url params="id: 'txtURL', value: view().custom.url, maxLength: 80" />
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w1">Links</label>
                    <table class="ala-form-table">
                        <thead>
                            <tr><th>Text</th><th>URL</th><th></th></tr>
                        </thead>
                        <tbody data-bind="foreach: view().custom.headerLinks">
                            <tr>
                                <td><input-text params="value: label, maxLength: 80, class: 'shorter'" /></td>
                                <td><input-text params="value: url, maxLength: 240" /></td>
                                <td><button class="ala-button-lite-red" title="Remove"><i class="icon-cancel"></i></button></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td><input-text params="value: newLinkLabel, maxLength: 80, class: 'shorter'" /></td>
                                <td><input-text params="value: newLinkURL, maxLength: 240" /></td>
                                <td><button class="ala-button-lite" title="Add Link"><i class="icon-plus-circled"></i>Add</button></td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
                <br />
                <h3>Footer</h3>
                <div class="ala-form-row">
                    <label class="ala-form-label-w2" for="txtCopyright">Copyright</label> 
                    <input-text params="id: 'txtCopyright', value: view().custom.copyright, maxLength: 80" />
                </div>
            </div>
            <div class="ala-tab-content" data-bind="visible: tabs.isColumnsSelected">
                <h2 class="ala-screen-reader-only">Columns</h2>
            </div>
            <div class="ala-tab-content" data-bind="visible: tabs.isSecuritySelected">
                <h2 class="ala-screen-reader-only">Security</h2>
                <div class="ala-form-description">
                    The <code>View Key</code> is an access key used to display this customized view to your clients. It should
                    be treated as a specialized API key, usable to access saved Event Log data but not make changes.
                </div>
                <div class="ala-form-row">
                    <label class="ala-form-label-w2">View Key</label> 
                    <span class="ala-form-input-faux" data-bind="text: view().key" /><br/>
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
