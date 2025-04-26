import API_URL from "../appConfig";

export default async function addActivity(formData, token){
    return fetch(`${API_URL}/SportActivities/Add-activity`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          SportActivityName: formData.sportActivityName,
          StartDate: formData.startDate,
          DayOfWeek: formData.dayOfWeek,
          StartHour: formData.startHour,
          DurationInMinutes: formData.durationInMinutes,
          LevelName: formData.levelName,
          EmployeeId: formData.employeeId,
          ParticipantLimit: formData.participantLimit,
          CourtName: formData.courtName,
          CostWithoutEquipment: formData.costWithoutEquipment,
          CostWithEquipment: formData.costWithEquipment,
        }),
        credentials: 'include',
      });
}