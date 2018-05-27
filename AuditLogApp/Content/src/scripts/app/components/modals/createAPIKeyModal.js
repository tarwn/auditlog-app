export default {
    name: 'modal-create-apikey',
    viewModel: class CreateAPIKeyModal {
        constructor(params) {
            this.displayName = ko.observable;

        }
    },
    template: `
        <label for="apiKeyDisplayName">Display Name</label>
        <input type="text" id="apiKeyDisplayName" data-bind="value: displayName" />
    `
};
