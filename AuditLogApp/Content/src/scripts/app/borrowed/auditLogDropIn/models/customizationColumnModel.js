export default class CustomizationColumnModel {
    constructor(rawData) {
        this.order = rawData.order;
        this.label = rawData.label;

        this.lines = rawData.lines.reduce((acc, line) => {
            if (line != null && line.field != null) {
                acc.push(CustomizationColumnModel.parseLine(line));
            }
            return acc;
        }, []);

        this.hasMultipleRows = this.lines.length > 1;
    }

    static parseLine(line) {
        const split = line.field.split('[');
        if (split.length === 1) {
            return { field: split[0] };
        }
        else {
            return { field: split[0], format: split[1].replace(']', '') };
        }
    }
}
