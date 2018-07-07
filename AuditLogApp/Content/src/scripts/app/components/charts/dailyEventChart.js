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

            this.chart = {};

            this.subscriptions = [];
            this.subscriptions.push(this.data.subscribe(() => {
                this._refreshRender();
            }));

            this._initialRender();
        }

        dispose() {
            this.subscriptions.forEach((s) => {
                s.dispose();
            });
        }

        _initialRender() {
            const margin = {
                top: 20,
                right: 20,
                bottom: 70,
                left: 40
            };
            const width = 600 - margin.left - margin.right;
            const height = 300 - margin.top - margin.bottom;

            if (!this._initializeChart(width, height, margin)) {
                setTimeout(() => this._initialRender(), 50);
                return;
            }
            this.chart.isInitialized = true;

            this._render(width, height, margin);
        }

        _refreshRender() {
            if (!this.chart.isInitialized) {
                return;
            }

            const margin = {
                top: 20,
                right: 20,
                bottom: 70,
                left: 40
            };
            const width = 600 - margin.left - margin.right;
            const height = 300 - margin.top - margin.bottom;

            this._render(width, height, margin);
        }

        _initializeChart(width, height, margin) {
            // not visible
            if (d3.select(`#${this.id}`).empty()) {
                return false;
            }

            this.chart.svg = d3.select(`#${this.id}`)
                .append('svg')
                .attr('width', width + margin.left + margin.right)
                .attr('height', height + margin.top + margin.bottom);

            // x-axis scale
            this.chart.xScale = d3.scaleBand()
                .range([0, width])
                .round(true)
                .paddingInner(.1);

            // x-axis drawing
            this.chart.xAxis = d3.axisBottom(this.chart.xScale)
                .ticks(1, 'no data');
            this.chart.svg.append('g')
                .attr('transform', `translate(0, ${height})`)
                .attr("class", "x axis")
                .call(this.chart.xAxis);

            // y-axis scale
            // TODO: improve range and tick intervals w/ dynamic rounding
            this.chart.yScale = d3.scaleLinear()
                .range([height, 0]);

            // y-axis drawing
            this.chart.yAxis = d3.axisLeft(this.chart.yScale)
                .ticks(0);
            this.chart.svg.append('g')
                .attr('transform', 'translate(0,0)')
                .attr('class', 'y axis')
                .call(this.chart.yAxis);

            return true;
        }

        _render(width, height, margin) {
            const { svg, xScale, xAxis, yScale, yAxis } = this.chart;

            // update the scales from data
            xScale.domain(this.data().map(d => d.date.valueOf()));
            xAxis.tickFormat((d) => new Date(d).getUTCDate());
            svg.select('.x.axis').transition().duration(300).call(xAxis);
            yScale.domain([0, this.eventCountMax()]);
            yAxis.ticks(4);

            // bars
            const bars = svg.selectAll('.ala-chart-bar')
                .data(this.data(), d => d.date.valueOf());

            bars.enter()
                .append('rect')
                .attr('class', 'ala-chart-bar')
                .style('fill', 'steelblue')
                .attr('x', d => xScale(d.date.valueOf()))
                .attr('width', 15)
                .attr('y', d => yScale(d.count()))
                .attr('height', d => height - yScale(d.count()));

            bars.exit()
                .remove();

            // bars.transition()
            //     .duration(300)
            //     .attr('x', d => xScale(d.date.valueOf()))
            //     .attr('width', this.dataCount())
            //     .attr('y', d => yScale(d.count()))
            //     .attr('height', d => height - yScale(d.count()));

            // bar.append('text')
            //     .attr('x', d => xScale(d.date.valueOf()))
            //     .attr('y', height)
            //     .attr('dy', '.35em')
            //     .text(d => d.date.format('D'));
        }
    },
    template: `
        <div class="ala-daily-event-chart-area">
            <div data-bind="attr: { id: id }"></div>
        </div>
    `
};
