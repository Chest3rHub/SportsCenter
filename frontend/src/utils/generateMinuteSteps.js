export default function generateMinuteSteps(start, end, stepMinutes) {
    const steps = [];

    for (let i = start; i <= end; i += stepMinutes) {
        steps.push(i);
    }

    return steps;
}
