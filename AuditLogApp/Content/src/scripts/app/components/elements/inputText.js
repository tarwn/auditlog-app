export default {
    name: 'input-text',
    viewModel: class InputText {
        constructor(params) {
            if (typeof params.value !== 'function') {
                this.value = params.value.o;
            }
            else {
                this.value = params.value().o;
            }

            this.id = params.id;
            this.isShorter = params.isShorter;
            if (this.value.isValid) {
                this.isValid = this.value.isValid;
            }
            else {
                this.isValid = ko.pureComputed(() => {
                    return true;
                });
            }
        }
    },
    template: `
        <input type="text" class="ala-form-input" data-bind="attr: { id: id }, value: value, css: { 'ala-input-error': !isValid(), shorter: isShorter }" />
    `
};
