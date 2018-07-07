export default class DailyEventCount {
    constructor(date, count) {
        this.date = date;
        this.count = ko.observable(count);
    }

    incrementCount(additionalCount) {
        this.count(this.count() + additionalCount);
    }
}
