export default async function getWorkingHoursForSingleDay(date) {
    const today = new Date();
    console.log('Today', today)
    const selected = new Date(date);
    console.log('Date', date)
    console.log('Selected', selected)


    today.setHours(0, 0, 0, 0);
    selected.setHours(0, 0, 0, 0);

    const todayDay = today.getDay();
    const selectedDay = selected.getDay();

    const getMonday = (d) => {
        const day = d.getDay();
        const diff = (day === 0 ? -6 : 1) - day; 
        const monday = new Date(d);
        monday.setDate(d.getDate() + diff);
        return monday;
    };

    const currentWeekStart = getMonday(today);
    const selectedWeekStart = getMonday(selected);

    const msInWeek = 7 * 24 * 60 * 60 * 1000;
    const weekOffset = Math.round((selectedWeekStart - currentWeekStart) / msInWeek);

    console.log("Week offset", weekOffset)

return weekOffset;
}
