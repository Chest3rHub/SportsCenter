import { Box, Typography, Modal, Button } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import { SportsContext } from "../context/SportsContext";
import Header from "../components/Header";
import OrangeBackground from "../components/OrangeBackground";
import GreenButton from "../components/GreenButton";
import ChangePageButton from "../components/ChangePageButton";
import CustomInput from "../components/CustomInput";
import getYourTasks from "../api/getYourTasks";
import addTask from "../api/addTask";
import editTask from "../api/editTask";
import deleteTask from "../api/deleteTask";

export default function ToDoPage() {
    const { dictionary, token } = useContext(SportsContext);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [offset, setOffset] = useState(0);
    const [openModal, setOpenModal] = useState(false);
    const [currentTask, setCurrentTask] = useState(null);
    const [editMode, setEditMode] = useState(false);
    const [newTask, setNewTask] = useState({
        description: "",
        dateTo: ""
    });

    const fetchTasks = async () => {
        try {
            const response = await getYourTasks(token, offset);
            if (!response.ok) throw new Error('Failed to fetch tasks');
            const data = await response.json();
            setTasks(data);
        } catch (error) {
            console.error('Error:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        setLoading(true);
        fetchTasks();
    }, [offset]);

    const handleOpenAddModal = () => {
        setNewTask({ description: "", dateTo: "" });
        setEditMode(false);
        setOpenModal(true);
    };

    const handleOpenEditModal = (task) => {
        setCurrentTask(task);
        setNewTask({ description: task.description, dateTo: task.dateTo });
        setEditMode(true);
        setOpenModal(true);
    };

    const handleCloseModal = () => {
        setOpenModal(false);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewTask(prev => ({ ...prev, [name]: value }));
    };

    const handleAddTask = async () => {
        try {
            const response = await addTask(token, newTask);
            if (!response.ok) throw new Error('Failed to add task');
            setOffset(0);
            await fetchTasks();
            handleCloseModal();
        } catch (error) {
            console.error('Error adding task:', error);
        }
    };

    const handleEditTask = async () => {
        try {
            const response = await editTask(token, {
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
            const response = await deleteTask(token, taskToDelete.taskId);
            if (!response.ok) throw new Error('Failed to delete task');
            setOffset(0);
            await fetchTasks();
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

    const displayedTasks = tasks.slice(0, 6);

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
            }}>
                <Header>{dictionary.toDoPage.toDoLabel}</Header>
                
                <Box sx={{
                    minHeight: '65vh',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    backgroundColor: 'white',
                    padding: '2rem',
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
                                    padding: '0.5rem 0',
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
                                        <OrangeBackground sx={{ 
                                            padding: '0.2rem 1rem',
                                            minWidth: 'fit-content',
                                            height: '24px',
                                            display: 'flex',
                                            alignItems: 'center',
                                            justifyContent: 'center'
                                        }}>
                                            <Typography sx={{ color: 'black' }}>{task.dateTo}</Typography>
                                        </OrangeBackground>
                                    </Box>
                                    
                                    <Box sx={{ 
                                        width: '20%', 
                                        display: 'flex', 
                                        gap: '1rem',
                                        justifyContent: 'flex-end',
                                        paddingRight: '1rem'
                                    }}>
                                        <ChangePageButton
                                            onClick={() => handleOpenEditModal(task)}
                                            backgroundColor="#8edfb4"
                                            minWidth="80px"
                                            height="36px"
                                            fontSize="0.9rem"
                                            textColor="black"
                                        >
                                            {dictionary.toDoPage.editLabel}
                                        </ChangePageButton>
                                        <ChangePageButton
                                            onClick={() => handleDeleteTask(index)}
                                            backgroundColor="#F46C63"
                                            minWidth="80px"
                                            height="36px"
                                            fontSize="0.9rem"
                                            textColor="black"
                                        >
                                            {dictionary.toDoPage.deleteLabel}
                                        </ChangePageButton>
                                    </Box>
                                </Box>
                            ))
                        )}
                    </Box>
                    
                    <Box sx={{
                        display: "flex",
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        marginTop: '2rem',
                        paddingTop: '1.5rem',
                        borderTop: '1px solid #eee'
                    }}>
                        <ChangePageButton
                            disabled={offset === 0}
                            onClick={handlePreviousPage}
                            backgroundColor="#F46C63"
                            minWidth="12vw"
                        >
                            {dictionary.toDoPage.previousLabel}
                        </ChangePageButton>
                        
                        <GreenButton
                            onClick={handleOpenAddModal}
                            style={{
                                width: 'fit-content',
                                height: '2.8rem',
                                fontSize: '1rem',
                                padding: '0 2rem',
                                backgroundColor: '#8edfb4',
                                color: 'black',
                                fontWeight: 'bold'
                            }}
                        >
                            {dictionary.toDoPage.addTaskLabel}
                        </GreenButton>
                        
                        <ChangePageButton
                            disabled={tasks.length <= 6}
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
                        sx={{ 
                            marginBottom: '4rem',
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': { color: 'black' }
                        }}
                    />
                    
                    <CustomInput
                        label={dictionary.toDoPage.dueDateLabel}
                        name="dateTo"
                        type="date"
                        value={newTask.dateTo}
                        onChange={handleInputChange}
                        fullWidth
                        sx={{ 
                            marginBottom: '3.5rem',
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': { color: 'black' }
                        }}
                        InputLabelProps={{ shrink: true }}
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
                                fontWeight: 'bold'
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
                                fontWeight: 'bold'
                            }}
                            onClick={editMode ? handleEditTask : handleAddTask}
                        >
                            {dictionary.toDoPage.saveLabel}
                        </Button>
                    </Box>
                </Box>
            </Modal>
        </>
    );
}