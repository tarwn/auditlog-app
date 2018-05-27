import AuthenticationMethod from '../../models/authenticationMethodModel';
import CreateAPIKeyModal from '../modals/createAPIKeyModal';

export default {
    name: 'page-apikeys',
    viewModel: class APIKeysPage {
        constructor(params) {
            this._services = params.services;
            this._sitewideContext = params.sitewideContext;
            this._navigationContext = params.navigationContext;
            this.readyForDisplay = params.readyForDisplay;
            this.params = params;

            this.apikeys = ko.observableArray();

            this.initialize();
        }

        initialize() {
            this.getAPIKeys()
                .then(() => {
                    this.readyForDisplay(true);
                });
        }

        getAPIKeys() {
            return this._services.getAPIKeys()
                .then((data) => {
                    const apikeys = data.map(key => new AuthenticationMethod(key));
                    this.apikeys(apikeys);
                });
        }

        createAPIKey() {
            return this._sitewideContext.modalRequest(CreateAPIKeyModal.name, )
                .then((result, apikey) => {
                    if (result === 'ok') {
                        return this.saveNewKey(apikey);
                    }
                    return this.getAPIKeys();
                });
        }

        // dispose() {
        // }
    },
    template: `
        <h1>API Keys</h1>
        <div class="ala-page-description">
            <p>API Keys grant Read and Write access to the API, critical for sending events from custom or pre-built audit libraries.</p>
        </div>
        <div class="ala-table-actions">
            <input type="button" style="ala-button ala-button-plus" value="Create API Key" data-bind="click: createAPIKey"/>
        </div>
        <table>
            <thead>
                <tr><th>API Key</th><th>Consumer Id</th><th>Consumer Secret</th><th>Created On</th><th>Created By</th><th></th></tr>
            </thead>
            <tbody data-bind="foreach: apikeys">
                <tr>
                    <td data-bind="text: displayName"></td>
                    <td data-bind="text: id"></td>
                    <td data-bind="text: display.createdOn"></td>
                    <td data-bind="text: display.createdBy"></td>
                    <td><button style="ala-button-red"><i class="icon-cancel"></i>Revoke</button></td>
                </tr>
            </tbody>
        </table>
    `
};
