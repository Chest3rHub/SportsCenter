export default function generateTime(start, end, stepMinutes) {
    const times = [];
    const [startHour, startMin] = start.split(":").map(Number);
    const [endHour, endMin] = end.split(":").map(Number);
  
    let current = new Date();
    current.setHours(startHour, startMin, 0, 0);
  
    const endTime = new Date();
    endTime.setHours(endHour, endMin, 0, 0);
  
    while (current <= endTime) {
      const hours = current.getHours().toString().padStart(2, "0");
      const minutes = current.getMinutes().toString().padStart(2, "0");
      times.push(`${hours}:${minutes}`);
      current.setMinutes(current.getMinutes() + stepMinutes);
    }
  
    return times;
  }
  