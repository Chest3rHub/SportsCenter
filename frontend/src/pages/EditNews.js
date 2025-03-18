import React, { useState, useContext, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import editNews from '../api/editNews';
import CustomInput from '../components/CustomInput';
import { Box } from '@mui/material';
import parseStringToDate from '../utils/parseStringToDate';


export default function EditNews() {

  const location = useLocation(); 
  const { oneNewsDetails } = location.state || {};

  console.log(oneNewsDetails);

  const { dictionary, toggleLanguage, token } = useContext(SportsContext);

  //na razie bez zdjecia (mozna dodac)
  const [formData, setFormData] = useState({
    id: oneNewsDetails.id,
    title: oneNewsDetails.title,
    content: oneNewsDetails.content,
    validFrom: oneNewsDetails.validFrom ? parseStringToDate(oneNewsDetails.validFrom) : '',
    validUntil: oneNewsDetails.validUntil ? parseStringToDate(oneNewsDetails.validUntil) : '',
  });
  console.log(formData);

    const [titleError, setTitleError] = useState(false);
    
    const [contentError, setContentError] = useState(false);
  
    const [validFromError, setValidFromError] = useState(false);
  
    const [validUntilError, setValidUntilError] = useState(false);

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

    return isValid;
  };

  function handleError(textToDisplay) {
    navigate('/error', {
      state: { message: textToDisplay }
    });
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
          console.log(errorData);
          handleError('Blad edytowania aktualności... sprawdz konsole');
        } else {
         // navigate('/add-news');
        }
  
      } catch (error) {
        console.error('Błąd edytowania aktualności:', error);
      }
    };

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
                <GreenButton type="submit">{dictionary.editNewsPage.saveLabel}</GreenButton>
              </Box>
            </form>
          </OrangeBackground>
        </GreenBackground>
      </>
    );
}
