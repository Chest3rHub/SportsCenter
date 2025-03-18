import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import addNews from '../api/addNews';
import CustomInput from '../components/CustomInput';
import { Box } from '@mui/material';
function AddNews() {

  const { dictionary, toggleLanguage, token } = useContext(SportsContext);

  //na razie bez zdjecia (mozna dodac)
  const [formData, setFormData] = useState({
    title: '',
    content: '',
    validFrom: '',
    validUntil: '',
  });

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
        const response = await addNews(formData, token);
  
        if (!response.ok) {
          const errorData = await response.json();
          console.log(errorData);
          handleError('Blad dodawania aktualności... sprawdz konsole');
        } else {
          navigate('/add-news');
        }
  
      } catch (error) {
        console.error('Błąd dodawania aktualności:', error);
      }
    };

  return (
      <>
        <GreenBackground height={"55vh"} marginTop={"2vh"}>
          <Header>{dictionary.addNewsPage.newsLabel}</Header>
          <OrangeBackground width="70%">
            <form onSubmit={handleSubmit}>
              <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
                <CustomInput
                  label={dictionary.addNewsPage.titleLabel}
                  type="text"
                  id="title"
                  name="title"
                  fullWidth
                  value={formData.title}
                  onChange={handleChange}
                  error={titleError}
                  helperText={titleError ? dictionary.addNewsPage.titleError : ""}
                  required
                  size="small"
                />
                <CustomInput
                  label={dictionary.addNewsPage.contentLabel}
                  type="text"
                  id="content"
                  name="content"
                  fullWidth
                  value={formData.content}
                  onChange={handleChange}
                  error={contentError}
                  helperText={contentError ? dictionary.addNewsPage.contentError : ""}
                  required
                  size="small"
                />
                <CustomInput
                  label={dictionary.addNewsPage.validFromLabel}
                  type="date"
                  id="validFrom"
                  name="validFrom"
                  fullWidth
                  value={formData.validFrom}
                  onChange={handleChange}
                  error={validFromError}
                  helperText={validFromError ? dictionary.addNewsPage.validFromError : ""}
                  size="small"
                  InputLabelProps={{
                    shrink: true
                  }}
                />
                <CustomInput
                  label={dictionary.addNewsPage.validUntilLabel}
                  type="date"
                  id="validUntil"
                  name="validUntil"
                  fullWidth
                  value={formData.validUntil}
                  onChange={handleChange}
                  error={validUntilError}
                  helperText={validUntilError ? dictionary.addNewsPage.validUntilError : ""}
                  size="small"
                  InputLabelProps={{
                    shrink: true
                  }}
                />
                <GreenButton type="submit">{dictionary.addNewsPage.saveLabel}</GreenButton>
              </Box>
            </form>
          </OrangeBackground>
        </GreenBackground>
      </>
    );
}

export default AddNews;