export default class DailyEventCount {
    constructor(date, count) {
        this.date = date;
        this.count = ko.observable(count);
    }

    setCount(newCount) {
        this.count(newCount);
    }
}
