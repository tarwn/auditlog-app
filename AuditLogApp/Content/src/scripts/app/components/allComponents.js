import dailyEventChart from './charts/dailyEventChart';
import inputText from './elements/inputText';
import inputURL from './elements/inputURL';
import sampleViewIFrame from './pages/views/sampleViewIFrame';

// include all components except for pages, which are registered during routing

export default [
    dailyEventChart,
    inputText,
    inputURL,
    sampleViewIFrame
];
