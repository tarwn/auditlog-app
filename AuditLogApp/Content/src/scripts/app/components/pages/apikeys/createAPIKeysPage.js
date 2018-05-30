import AuthenticationMethod from '../../../models/authenticationMethodModel';

export default {
    name: 'page-apikeys-create',
    viewModel: class APIKeysCreatePage {
        constructor(params) {
            this._services = params.services;
            this._sitewideContext = params.sitewideContext;
            this._navigationContext = params.navigationContext;
            this.readyForDisplay = params.readyForDisplay;
            this.params = params;

            // Workflow
            this.steps = {};
            this.steps.currentStep = ko.observable(1);
            this.steps.isStep1 = ko.pureComputed(() => this.steps.currentStep() === 1);
            this.steps.isStep2 = ko.pureComputed(() => this.steps.currentStep() === 2);

            // Step 1
            this.displayName = ko.observable('Memorable name');
            this.displayNameIsValid = ko.pureComputed(() => {
                const rawValue = (this.displayName() || '').trim();
                // hardcoded to match DB and server-side CreateAPIKeyModel
                return rawValue.length > 0 && rawValue.length <= 80;
            });
            this.isCreatingAPIKey = ko.observable(false);

            // Step 2
            this.apikey = ko.observable();

            this.initialize();
        }

        initialize() {
            this.readyForDisplay(true);
        }

        createAPIKey() {
            this.isCreatingAPIKey(true);
            this._services.createAPIKey(this.displayName())
                .then((data) => {
                    console.log(data);
                    this.apikey(new AuthenticationMethod(data));
                    this.steps.currentStep(2);
                    this.isCreatingAPIKey(false);
                });
        }

        // dispose() {
        // }
    },
    template: `
        <h1>Create API Key</h1>
        <div data-bind="visible: steps.isStep1">
            <label for="txtDisplayName">Display Name</label> 
            <input type="text" id="txtDisplayName" data-bind="value: displayName, css: { 'ala-input-error': !displayNameIsValid() }" /><br/>
            <div class="ala-input-instructions">A good name describes the purpose of the key and helps you revoke the correct one later, if needed.</div>
            <input type="button" value="Create" class="ala-button" data-bind="click: createAPIKey, enabled: !isCreatingAPIKey()" />
        </div>
        <div data-bind="if: steps.isStep2">
            <label for="spDisplayName">Display Name</label>
                <span class="ala-input-faux" data-bind="text: apikey().displayName"></span></br>
            <label for="spAPIKeyId">Id</label>
                <span class="ala-input-faux" data-bind="text: apikey().id"></span></br>
            <label for="spAPIKeySecret">Secret</label>
                <span class="ala-input-faux" data-bind="text: apikey().secret"></span></br>
            <br/>
            <div class="ala-input-instructions">This is the last time we'll show the value of the secret, copy it into your configuration now.</div>
            <a href="/configure/apikeys" title="Return to API Keys list">Back to API Keys list</a>
        </div>
    `
};
