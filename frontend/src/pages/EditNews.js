import React, { useState, useContext } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import editNews from '../api/editNews';
import CustomInput from '../components/CustomInput';
import { Box, Typography, Avatar } from '@mui/material';
import parseStringToDate from '../utils/parseStringToDate';
import NewsButton from '../components/buttons/NewsButton';
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

export default function EditNews() {

    const location = useLocation();
    const { oneNewsDetails, offsetFromLocation } = location.state || {};

    const { dictionary, toggleLanguage, token } = useContext(SportsContext);

    //na razie bez zdjecia (mozna dodac)
    const [formData, setFormData] = useState({
        id: oneNewsDetails.id,
        title: oneNewsDetails.title,
        content: oneNewsDetails.content,
        validFrom: oneNewsDetails.validFrom ? parseStringToDate(oneNewsDetails.validFrom) : '',
        validUntil: oneNewsDetails.validUntil ? parseStringToDate(oneNewsDetails.validUntil) : '',
    });

    const [titleError, setTitleError] = useState(false);

    const [contentError, setContentError] = useState(false);

    const [validFromError, setValidFromError] = useState(false);

    const [validUntilError, setValidUntilError] = useState(false);

    const [openSuccessBackdrop, setOpenSuccessBackdrop] = useState(false);
    const [openFailureBackdrop, setOpenFailureBackdrop] = useState(false);


    const handleCloseSuccess = () => {
        setOpenSuccessBackdrop(false);
    };
    const handleCloseFailure = () => {
        setOpenFailureBackdrop(false);
    };
    const handleOpenSuccess = () => {
        setOpenSuccessBackdrop(true);
    };
    const handleOpenFailure = () => {
        setOpenFailureBackdrop(true);
    };

    const validateForm = () => {
        let isValid = true;


        if (formData.title.length > 20) {
            isValid = false;
            setTitleError(true);
        } else {
            setTitleError(false);
        }


        if (formData.content.length > 4000) {
            isValid = false;
            setContentError(true);
        } else {
            setContentError(false);
        }
        const validFromDate = new Date(formData.validFrom);
        const validUntilDate = new Date(formData.validUntil);

    
        if (validFromDate >= validUntilDate) {
            isValid = false;
            setValidFromError(true);
            setValidUntilError(true);
        } else {
            setValidFromError(false);
            setValidUntilError(false);
        }

        if(!formData.validFrom || validFromDate >= validUntilDate){
            isValid = false;
            setValidFromError(true);
        } else {
            setValidFromError(false);
        }

        return isValid;
    };

    function handleError(textToDisplay) {
        // navigate('/error', {
        //     state: { message: textToDisplay }
        // });
        handleOpenFailure();
    }

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }


        try {
            const response = await editNews(formData, token);

            if (!response.ok) {
                const errorData = await response.json();
              //  console.log(errorData);
                //handleError('Blad edytowania aktualności... sprawdz konsole');
                
                handleOpenFailure();
            } else {
                // navigate('/add-news');
                handleOpenSuccess();
            }

        } catch (error) {
          //error('Błąd edytowania aktualności:', error);
            handleOpenFailure();

        }
    };
    function handleCancel() {
        navigate('/news', {
            state: { offsetFromLocation }  
          });
    }

    return (
        <>
            <GreenBackground height={"55vh"} marginTop={"2vh"}>
                <Header>{dictionary.editNewsPage.newsLabel}</Header>
                <OrangeBackground width="70%">
                    <form onSubmit={handleSubmit}>
                        <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
                            <CustomInput
                                label={dictionary.editNewsPage.titleLabel}
                                type="text"
                                id="title"
                                name="title"
                                fullWidth
                                value={formData.title}
                                onChange={handleChange}
                                error={titleError}
                                helperText={titleError ? dictionary.editNewsPage.titleError : ""}
                                required
                                size="small"
                            />
                            <CustomInput
                                label={dictionary.editNewsPage.contentLabel}
                                type="text"
                                id="content"
                                name="content"
                                fullWidth
                                value={formData.content}
                                onChange={handleChange}
                                error={contentError}
                                helperText={contentError ? dictionary.editNewsPage.contentError : ""}
                                required
                                size="small"
                            />
                            <CustomInput
                                label={dictionary.editNewsPage.validFromLabel}
                                type="date"
                                id="validFrom"
                                name="validFrom"
                                fullWidth
                                value={formData.validFrom}
                                onChange={handleChange}
                                error={validFromError}
                                helperText={validFromError ? dictionary.editNewsPage.validFromError : ""}
                                size="small"
                                InputLabelProps={{
                                    shrink: true
                                }}
                            />
                            <CustomInput
                                label={dictionary.editNewsPage.validUntilLabel}
                                type="date"
                                id="validUntil"
                                name="validUntil"
                                fullWidth
                                value={formData.validUntil}
                                onChange={handleChange}
                                error={validUntilError}
                                helperText={validUntilError ? dictionary.editNewsPage.validUntilError : ""}
                                size="small"
                                InputLabelProps={{
                                    shrink: true
                                }}
                            />
                            <Box sx={{
                                display: "flex",
                                flexDirection: "row",
                                justifyContent: 'center',
                                columnGap: "4vw"
                                // ten pierwszy nie jest zielony tylko czerwony xd ale juz niech tak zostanie poki co 
                                // ewentualnie mozna go zmienic na taki pomaranczowy jak przycisk do edycji newsa
                            }}>
                                <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.editNewsPage.returnLabel}</GreenButton>
                                <GreenButton type="submit" style={{ maxWidth: "10vw" }}>{dictionary.editNewsPage.saveLabel}</GreenButton>
                            </Box>
                        </Box>

                        <Backdrop
                            sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
                            open={openSuccessBackdrop}
                            onClick={handleCloseSuccess}
                        >
                            <Box sx={{
                                backgroundColor: "white",
                                margin: 'auto',
                                minWidth: '30vw',
                                minHeight:'30vh',
                                borderRadius: '20px',
                                boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                                
                            }}>
                                <Box>
                                <Typography sx={{
                                    color: 'green',
                                    fontWeight:'Bold',
                                    fontSize: '3rem',
                                    marginTop:'2vh',

                                }}>
                                    {dictionary.editNewsPage.successLabel}
                                </Typography>
                                </Box>
                                <Box>
                                <Typography sx={{
                                    color: 'black',
                                    fontSize:'1.5rem',
                                }}>
                                    {dictionary.editNewsPage.savedSuccessLabel}
                                </Typography>
                                </Box>
                                <Box>
                                <Typography sx={{
                                    color: 'black',
                                    fontSize:'1.5rem',
                                }}>
                                  {dictionary.editNewsPage.clickAnywhereLabel}
                                </Typography>
                                </Box>
                                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                                <Avatar sx={{ width: "7rem", height: "7rem" }}>
                                    <SentimentSatisfiedIcon sx={{ fontSize: "7rem", color: 'green', stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }} />
                                </Avatar>
                            </Box>
                            </Box>
                        </Backdrop>
                        <Backdrop
                            sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
                            open={openFailureBackdrop}
                            onClick={handleCloseFailure}
                        >
                            <Box sx={{
                                backgroundColor: "white",
                                margin: 'auto',
                                minWidth: '30vw',
                                minHeight:'30vh',
                                borderRadius: '20px',
                                boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                                
                            }}>
                                <Box>
                                <Typography sx={{
                                    color: 'red',
                                    fontWeight:'Bold',
                                    fontSize: '3rem',
                                    marginTop:'2vh',

                                }}>
                                    {dictionary.editNewsPage.failureLabel}
                                </Typography>
                                </Box>
                                <Box>
                                <Typography sx={{
                                    color: 'black',
                                    fontSize:'1.5rem',
                                }}>
                                    {dictionary.editNewsPage.savedFailureLabel}
                                </Typography>
                                </Box>
                                <Box>
                                <Typography sx={{
                                    color: 'black',
                                    fontSize:'1.5rem',
                                }}>
                                  {dictionary.editNewsPage.clickAnywhereFailureLabel}
                                </Typography>
                                </Box>
                                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                                <Avatar sx={{ width: "7rem", height: "7rem" }}>
                                    <SentimentDissatisfiedIcon sx={{ fontSize: "7rem", color: 'red', stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }} />
                                </Avatar>
                            </Box>
                            </Box>
                        </Backdrop>
                    </form>
                </OrangeBackground>
            </GreenBackground>
        </>
    );
}
