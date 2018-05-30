import AuthenticationMethod from '../../../models/authenticationMethodModel';

export default {
    name: 'page-apikeys-list',
    viewModel: class APIKeysListPage {
        constructor(params) {
            this._services = params.services;
            this._sitewideContext = params.sitewideContext;
            this._navigationContext = params.navigationContext;
            this.readyForDisplay = params.readyForDisplay;

            this.apikeys = ko.observableArray();
            this.displayableAPIKeys = ko.pureComputed(() => {
                // initially just filter out revoked items
                const filter = item => !item.isRevoked;
                return ko.utils.arrayFilter(this.apikeys(), filter);
            });

            this.isRevokingKey = ko.observable(false);

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

        revokeAPIKey(key) {
            this.isRevokingKey(true);
            this._services.revokeAPIKey(key.id)
                .then(() => {
                    this.isRevokingKey(false);
                })
                .then(() => this.getAPIKeys());
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
            <a href="/configure/apikeys/create" title="Create API Key" style="ala-button"><i class="icon-plus-circled"></i>Create API Key</a>
        </div>
        <table>
            <thead>
                <tr><th>API Key</th><th>Consumer Id</th><th>Created On</th><th></th></tr>
            </thead>
            <tbody data-bind="foreach: displayableAPIKeys">
                <tr>
                    <td data-bind="text: displayName"></td>
                    <td data-bind="text: id"></td>
                    <td data-bind="text: display.creationTime"></td>
                    <td><button style="ala-button-red" data-bind="click: $parent.revokeAPIKey.bind($parent), enabled: !$parent.isRevokingKey"><i class="icon-cancel"></i>Revoke</button></td>
                </tr>
            </tbody>
        </table>
    `
};
