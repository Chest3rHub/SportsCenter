const MAX_ACTIVITY_TIME_IN_HOURS = 5;

export default function getEndTimeOptions(formData, closeHour){
  if (!formData.startTime) return [];

  const [startHour, startMinute] = formData.startTime.split(':').map(Number);
  const baseDate = new Date(2000, 0, 1, startHour, startMinute);

  const maxEndDate = new Date(baseDate.getTime() + MAX_ACTIVITY_TIME_IN_HOURS * 60 * 60 * 1000);

  const [closeHourNum, closeMinuteNum] = closeHour.split(':').map(Number);
  const closeDate = new Date(2000, 0, 1, closeHourNum, closeMinuteNum);

  const finalMaxEndDate = maxEndDate < closeDate ? maxEndDate : closeDate;

  const minEndDate = new Date(baseDate.getTime() + 30 * 60 * 1000);


  const pad = (n) => n.toString().padStart(2, '0');

  let times = [];
  for (
    let d = minEndDate;
    d <= finalMaxEndDate;
    d = new Date(d.getTime() + 30 * 60 * 1000)
  ) {
    const timeStr = `${pad(d.getHours())}:${pad(d.getMinutes())}`;
    times.push(timeStr);
  }
  return times;
};
