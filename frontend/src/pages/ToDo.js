import { Box, Typography, Modal, Button, MenuItem, Select, FormControl, InputLabel } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import { SportsContext } from "../context/SportsContext";
import Header from "../components/Header";
import OrangeBackground from "../components/OrangeBackground";
import GreenButton from "../components/buttons/GreenButton";
import ChangePageButton from "../components/buttons/ChangePageButton";
import TasksButton from "../components/buttons/TasksButton";
import CustomInput from "../components/CustomInput";
import getYourTasks from "../api/getYourTasks";
import selfAddTask from "../api/selfAddTask";
import editTask from "../api/editTask";
import deleteTask from "../api/deleteTask";
import getEmployees from "../api/getEmployees";
import addTask from "../api/addTask";

export default function ToDoPage() {
    const { dictionary, role } = useContext(SportsContext);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [offset, setOffset] = useState(0);
    const [openModal, setOpenModal] = useState(false);
    const [openEmployeeModal, setOpenEmployeeModal] = useState(false);
    const [currentTask, setCurrentTask] = useState(null);
    const [editMode, setEditMode] = useState(false);
    const [employees, setEmployees] = useState([]);
    const [selectedEmployee, setSelectedEmployee] = useState('');
    const [newTask, setNewTask] = useState({
        description: "",
        dateTo: ""
    });

    const maxTasksPerPage = 7;

    const tasksRequiredToEnablePagination = 8;

    const isOwner = role === "Wlasciciel";

    const fetchTasks = async () => {
        try {
            const response = await getYourTasks(offset);
            if (!response.ok) {
                if (response.status === 404) {
                    setTasks([]);
                    return;
                }
                throw new Error('Failed to fetch tasks');
            }
            const data = await response.json();
            setTasks(data);
        } catch (error) {
            console.error('Error:', error);
            setTasks([]);
        } finally {
            setLoading(false);
        }
    };

    const fetchEmployees = async () => {
        try {
            const response = await getEmployees(0);
            if (!response.ok) throw new Error('Failed to fetch employees');
            const data = await response.json();
            const adminEmployees = data.filter(emp => emp.role === "Pracownik administracyjny");
            setEmployees(adminEmployees);
        } catch (error) {
            console.error('Error:', error);
        }
    };

    useEffect(() => {
        setLoading(true);
        fetchTasks();
        if (isOwner) {
            fetchEmployees();
        }
    }, [offset]);

    const handleOpenAddModal = () => {
        setNewTask({ description: "", dateTo: "" });
        setEditMode(false);
        setOpenModal(true);
    };

    const handleOpenEmployeeModal = () => {
        setNewTask({ description: "", dateTo: "" });
        setSelectedEmployee('');
        setOpenEmployeeModal(true);
    };

    const handleOpenEditModal = (task) => {
        setCurrentTask(task);
        setNewTask({ description: task.description, dateTo: task.dateTo });
        setEditMode(true);
        setOpenModal(true);
    };

    const handleCloseModal = () => {
        setOpenModal(false);
        setOpenEmployeeModal(false);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewTask(prev => ({ ...prev, [name]: value }));
    };

    const handleEmployeeChange = (e) => {
        setSelectedEmployee(e.target.value);
    };

    const validateDate = (dateString) => {
        const selectedDate = new Date(dateString);
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        return selectedDate >= today;
    };

    const handleAddTask = async () => {
        if (!validateDate(newTask.dateTo)) {
            alert(dictionary.toDoPage.invalidDateError);
            return;
        }

        try {
            const response = await selfAddTask(newTask);
            if (!response.ok) throw new Error('Failed to add task');
            setOffset(0);
            await fetchTasks();
            handleCloseModal();
        } catch (error) {
            console.error('Error adding task:', error);
        }
    };

    const handleAddEmployeeTask = async () => {
        if (!validateDate(newTask.dateTo)) {
            alert(dictionary.toDoPage.invalidDateError);
            return;
        }
        if (!selectedEmployee) {
            alert(dictionary.toDoPage.selectEmployeeError);
            return;
        }

        try {
            const response = await addTask({
                ...newTask,
                employeeId: selectedEmployee
            });
            if (!response.ok) throw new Error('Failed to add task');
            setOffset(0);
            await fetchTasks();
            handleCloseModal();
        } catch (error) {
            console.error('Error adding employee task:', error);
        }
    };

    const handleEditTask = async () => {
        if (!validateDate(newTask.dateTo)) {
            alert(dictionary.toDoPage.invalidDateError);
            return;
        }

        try {
            const response = await editTask({
                ...newTask,
                taskId: currentTask.taskId
            });
            if (!response.ok) throw new Error('Failed to edit task');
            fetchTasks();
            handleCloseModal();
        } catch (error) {
            console.error('Error editing task:', error);
        }
    };

    const handleDeleteTask = async (taskIndex) => {
    try {
        const taskToDelete = tasks[taskIndex];
        if (!taskToDelete) throw new Error('Task not found');
        const response = await deleteTask(taskToDelete.taskId);
        if (!response.ok) throw new Error('Failed to delete task');
        
        const isLastTaskOnPage = tasks.length === 1;
        if (isLastTaskOnPage) {
            if (offset > 0) {
                setOffset(prev => prev - 1);
            } else {
                await fetchTasks();
            }
        } else {
            await fetchTasks();
        }
    } catch (error) {
        console.error('Error deleting task:', error);
    }
};

    const handleNextPage = () => {
        setOffset(prev => prev + 1);
    };

    const handlePreviousPage = () => {
        if (offset === 0) return;
        setOffset(prev => prev - 1);
    };

    const displayedTasks = tasks.slice(0, maxTasksPerPage);

    return (
        <>
            <Box sx={{
                width: '64%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                flexGrow: 1,
                marginLeft: 'auto',
                marginRight: 'auto',
                marginTop: '10vh',
            }}>
                <Header>{dictionary.toDoPage.toDoLabel}</Header>

                <Box sx={{
                    minHeight: '65vh',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    backgroundColor: 'white',
                    padding: '1rem',
                    display: 'flex',
                    flexDirection: 'column',
                }}>
                    <Box sx={{
                        display: 'flex',
                        width: '100%',
                        marginBottom: '1.5rem',
                        paddingBottom: '0.5rem',
                        borderBottom: '1px solid #eee',
                    }}>
                        <Typography variant="h6" sx={{
                            width: '60%',
                            fontWeight: 'bold',
                            fontSize: '1.1rem',
                            paddingLeft: '1rem',
                            color: 'black'
                        }}>
                            {dictionary.toDoPage.taskLabel}
                        </Typography>
                        <Typography variant="h6" sx={{
                            width: '20%',
                            fontWeight: 'bold',
                            fontSize: '1.1rem',
                            color: 'black',
                            textAlign: 'center',
                            paddingRight: '20%'
                        }}>
                            {dictionary.toDoPage.dueDateLabel}
                        </Typography>
                        <Button onClick={handleOpenAddModal}
                            variant="contained"
                            sx={{
                                fontSize: '2rem',
                                maxHeight: '2rem',
                                color: 'black',
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                borderRadius: '30px',
                                marginTop: '-0.2rem'

                            }}>
                            +
                        </Button>
                    </Box>

                    <Box sx={{ flexGrow: 1 }}>
                        {loading ? (
                            <Typography sx={{ color: 'black' }}>
                                {dictionary.toDoPage.loadingLabel}
                            </Typography>
                        ) : displayedTasks.length === 0 ? (
                            <Typography sx={{ color: 'black' }}>
                                {dictionary.toDoPage.noTasksLabel}
                            </Typography>
                        ) : (
                            displayedTasks.map((task, index) => (
                                <Box key={task.taskId} sx={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    marginBottom: '1rem',
                                    
                                }}>
                                    <Typography sx={{
                                        width: '60%',
                                        textAlign: 'left',
                                        paddingLeft: '1rem',
                                        color: 'black'
                                    }}>
                                        • {task.description}
                                    </Typography>

                                    <Box sx={{
                                        width: '20%',
                                        display: 'flex',
                                        justifyContent: 'center',
                                        position: 'relative',
                                        left: '0.5%'
                                    }}>
                                        <Box sx={{
                                            backgroundColor: '#FFE3B3',
                                            borderRadius: '20px',
                                            padding:'8px 20px',
                                            marginLeft:'-11vh',
                                            boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                                        }}>
                                        <Typography sx={{ color: 'black' }}>{task.dateTo}</Typography>
                                        </Box>  
                                    </Box>

                                    <Box sx={{
                                        width: '20%',
                                        display: 'flex',
                                        gap: '1rem',
                                        justifyContent: 'flex-end',
                                        paddingRight: '1rem'
                                    }}>
                                        <TasksButton
                                            onClick={() => handleOpenEditModal(task)}
                                            disabled={task.assigningEmpId !== task.empId}
                                            backgroundColor={task.assigningEmpId !== task.empId ? "#e0e0e0" : "#8edfb4"}
                                            textColor={task.assigningEmpId !== task.empId ? "#9e9e9e" : "black"}
                                            minWidth="80px"
                                            height="36px"
                                            fontSize="0.9rem"
                                        >
                                            {dictionary.toDoPage.editLabel}
                                        </TasksButton>
                                        <TasksButton
                                            onClick={() => handleDeleteTask(index)}
                                            backgroundColor="#F46C63"
                                            minWidth="80px"
                                            height="36px"
                                            fontSize="0.9rem"
                                            textColor="black"
                                        >
                                            {dictionary.toDoPage.deleteLabel}
                                        </TasksButton>
                                    </Box>
                                </Box>
                            ))
                        )}
                    </Box>



                    {isOwner && (<Box sx={{
                        position: "fixed",
                        top: "12vh",
                        right: "3vw",
                        minWidth: "17vw"
                    }}>

                        <GreenButton
                            onClick={handleOpenEmployeeModal}
                            style={{


                                fontSize: '0.8rem',
                                padding: "3px 8px",
                                backgroundColor: '#8edfb4',
                                color: 'black',
                                fontWeight: 'bold',

                            }}
                        >
                            {dictionary.toDoPage.addEmployeeTaskLabel}
                        </GreenButton>



                    </Box>)}

                    <Box sx={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: 'center',
                        columnGap: "4vw",
                        alignItems: 'center',
                        justifyItems: 'center',
                        alignContent: 'center',
                        textAlign: 'center',
                    }}>
                        <ChangePageButton
                            disabled={offset === 0}
                            onClick={handlePreviousPage}
                            backgroundColor="#F46C63"
                            minWidth="12vw"
                        >
                            {dictionary.toDoPage.previousLabel}
                        </ChangePageButton>
                        <ChangePageButton
                            disabled={tasks.length<tasksRequiredToEnablePagination}
                            onClick={handleNextPage}
                            backgroundColor="#8edfb4"
                            minWidth="12vw"
                        >
                            {dictionary.toDoPage.nextLabel}
                        </ChangePageButton>
                    </Box>
                </Box>
            </Box>

            <Modal
                open={openModal}
                onClose={handleCloseModal}
                aria-labelledby="task-modal-title"
                aria-describedby="task-modal-description"
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <Box sx={{
                    backgroundColor: 'white',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    padding: '3rem',
                    width: '500px',
                    maxWidth: '90%',
                }}>
                    <Typography variant="h5" component="h2" sx={{
                        marginBottom: '2.5rem',
                        fontWeight: 'bold',
                        color: 'black'
                    }}>
                        {editMode ? dictionary.toDoPage.editTaskLabel : dictionary.toDoPage.addTaskLabel}
                    </Typography>

                    <CustomInput
                        label={dictionary.toDoPage.taskDescriptionLabel}
                        name="description"
                        value={newTask.description}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        variant={"outlined"}
                    />

                    <CustomInput
                        label={dictionary.toDoPage.dueDateLabel}
                        name="dateTo"
                        type="date"
                        value={newTask.dateTo}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                        inputProps={{
                            min: new Date().toISOString().split('T')[0]
                        }}
                    />

                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        marginTop: '1.5rem'
                    }}>
                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleCloseModal}
                        >
                            {dictionary.toDoPage.backLabel}
                        </Button>

                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#8edfb4',
                                '&:hover': { backgroundColor: '#7ecba3' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={editMode ? handleEditTask : handleAddTask}
                        >
                            {dictionary.toDoPage.saveLabel}
                        </Button>
                    </Box>
                </Box>
            </Modal>

            <Modal
                open={openEmployeeModal}
                onClose={handleCloseModal}
                aria-labelledby="employee-task-modal-title"
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <Box sx={{
                    backgroundColor: 'white',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    padding: '3rem',
                    width: '500px',
                    maxWidth: '90%',
                }}>
                    <Typography variant="h5" component="h2" sx={{
                        marginBottom: '2.5rem',
                        fontWeight: 'bold',
                        color: 'black'
                    }}>
                        {dictionary.toDoPage.addEmployeeTaskLabel}
                    </Typography>

                    <FormControl fullWidth sx={{ mb: 4 }}>
                        <InputLabel sx={{ color: 'black' }}>
                            {dictionary.toDoPage.selectEmployeeLabel}
                        </InputLabel>
                        <Select
                            value={selectedEmployee}
                            onChange={handleEmployeeChange}
                            label={dictionary.toDoPage.selectEmployeeLabel}
                            sx={{
                                color: 'black',
                                borderRadius: '40px',
                                '& .MuiOutlinedInput-root': {
                                  borderRadius: '40px',
                                },
                                '& fieldset': {
                                  borderRadius: '40px',
                                },
                              }}
                            
                        >
                            {employees.map((employee) => (
                                <MenuItem key={employee.id} value={employee.id}>
                                    {employee.fullName}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    <CustomInput
                        label={dictionary.toDoPage.taskDescriptionLabel}
                        name="description"
                        value={newTask.description}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                    />

                    <CustomInput
                        label={dictionary.toDoPage.dueDateLabel}
                        name="dateTo"
                        type="date"
                        value={newTask.dateTo}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                        inputProps={{
                            min: new Date().toISOString().split('T')[0]
                        }}
                    />

                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        marginTop: '1.5rem'
                    }}>
                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleCloseModal}
                        >
                            {dictionary.toDoPage.backLabel}
                        </Button>

                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#8edfb4',
                                '&:hover': { backgroundColor: '#7ecba3' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleAddEmployeeTask}
                        >
                            {dictionary.toDoPage.saveLabel}
                        </Button>
                    </Box>
                </Box>
            </Modal>
        </>
    );
}