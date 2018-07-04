import DailyEventCount from './models/dailyEventCount';

export default {
    name: 'daily-event-chart',
    viewModel: class DailyEventChartViewModel {
        constructor(params) {
            this.id = `chart_${Math.round(Math.random() * 10000)}`;
            this.data = params.value.data;

            this.safeData = ko.pureComputed(() => {
                if (this.data() == null || this.data().length === 0) {
                    return [new DailyEventCount(moment().startOf('day'), 0)];
                }
                else {
                    return this.data();
                }
            });

            this.startDate = ko.pureComputed(() => {
                return this.safeData()[0].date;
            });
            this.endDate = ko.pureComputed(() => {
                return this.safeData().slice(-1)[0].date;
            });
            this.eventCountMax = ko.pureComputed(() => {
                return this.data().reduce((acc, d) => Math.max(acc, d.count()), 0);
            });
            this.dataCount = ko.pureComputed(() => {
                return this.safeData().length;
            });

            this.subscriptions = [];
            this.subscriptions.push(this.data.subscribe(() => {
                this.render();
            }));
        }

        dispose() {
            this.subscriptions.forEach((s) => {
                s.dispose();
            });
        }

        render() {
            console.log(this.data());
            const margin = {
                top: 20,
                right: 20,
                bottom: 70,
                left: 40
            };
            const width = 600 - margin.left - margin.right;
            const height = 300 - margin.top - margin.bottom;

            const xScale = d3.scaleUtc()
                .domain([this.startDate().valueOf(), this.endDate().valueOf()])
                .range([0, width]);

            console.log({ xScale, st: this.startDate().valueOf(), en: this.endDate().valueOf() });


            const yScale = d3.scaleLinear()
                .domain([0, this.eventCountMax()])
                .range([height, 0]);

            const svg = d3.select(`#${this.id}`)
                .append('svg')
                .attr('width', width + margin.left + margin.right)
                .attr('height', height + margin.top + margin.bottom);

            const bar = svg.selectAll('g')
                .data(this.data())
                .enter().append('g');

            bar.append('rect')
                .style('fill', 'steelblue')
                .attr('x', d => xScale(d.date.valueOf()))
                .attr('width', this.dataCount())
                .attr('y', d => yScale(d.count()))
                .attr('height', d => height - yScale(d.count()));

            bar.append('text')
                .attr('x', d => xScale(d.date.valueOf()))
                .attr('y', d => yScale(d.count()) - 3)
                .attr('dy', '.35em')
                .text(d => d.date.format('D'));
        }
    },
    template: `
        <div class="ala-daily-event-chart-area">
            <div data-bind="attr: { id: id }"></div>
        </div>
    `
};
