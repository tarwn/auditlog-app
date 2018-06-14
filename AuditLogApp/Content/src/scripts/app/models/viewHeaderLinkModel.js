import validation from '../extenders/validation';

export default class ViewHeaderLinkModel {
    constructor(rawData) {
        let data = rawData;
        if (rawData === null) {
            this.isRequired = ko.observable(false);
            data = { label: '', url: '' };
        }
        else {
            this.isRequired = ko.observable(false);
        }

        this.label = ko.observable(data.label).extend(validation.string(true, 40));
        this.url = ko.observable(data.url).extend(validation.url(true, 400));

        this.isValid = ko.pureComputed(() => {
            const isOptional = !this.isRequired();
            const isFilledIn = this.label.isValid() && this.url.isValid();
            return isOptional || isFilledIn;
        });
    }
}
