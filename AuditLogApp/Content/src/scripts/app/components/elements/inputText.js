export default {
    name: 'input-text',
    viewModel: class InputText {
        constructor(params) {
            if (typeof params.value !== 'function') {
                throw new Error(`Value is not an observable, InputText: ${ko.toJSON(params)}`);
            }

            this.id = params.id;
            this.value = params.value;
            this.maxLength = params.maxLength;
            this.extraClass = ko.pureComputed(() => params.class);
            this.isValid = ko.pureComputed(() => {
                return this.value() != null &&
                    this.value().length <= this.maxLength;
            });
        }
    },
    template: `
        <input type="text" class="ala-form-input" data-bind="attr: { id: id }, value: value, css: { 'ala-input-error': !isValid() }, css: extraClass" />
    `
};
