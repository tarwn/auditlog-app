import validation from '../extenders/validation';

export default class ViewColumnConfigurationModel {
    constructor(rawData) {
        this.order = ko.observable(rawData.order);
        this.label = ko.observable(rawData.label).extend(validation.string(true, 40));
        this.lines = ko.observableArray([
            { field: ko.observable(rawData.lines[0].field) }
        ]);

        if (rawData.lines.length > 1) {
            this.lines.push({ field: ko.observable(rawData.lines[1].field) });
        }
        else {
            this.lines.push({ field: ko.observable() });
        }

        this.isValid = ko.pureComputed(() => {
            return this.label.isValid();
        });
    }

    static getEmpty() {
        return new ViewColumnConfigurationModel({ order: 0, label: 'New Column', lines: [{ field: null }] });
    }
}
