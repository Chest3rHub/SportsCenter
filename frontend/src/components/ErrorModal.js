import React from 'react';

const ErrorModal = ({ open, onClose, errorMessage, title = 'Błąd' }) => {
  if (!open) return null;

  const modalOverlayStyle = {
    position: 'fixed',
    top: 0,
    left: 0,
    width: '100%',
    height: '100%',
    backgroundColor: 'rgba(0, 0, 0, 0.6)',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    zIndex: 9999,
  };

  const modalContentStyle = {
    background: 'linear-gradient(to right, #0AB68B, #FFE3B3)',
    borderRadius: '10px',
    padding: '20px',
    width: '80%',
    maxWidth: '400px',
    textAlign: 'center',
    color: 'white',
  };

  const modalHeaderStyle = {
    fontSize: '24px',
    marginBottom: '20px',
    color: 'black',
  };

  const modalBodyStyle = {
    fontSize: '16px',
    marginBottom: '20px',
    color: 'black',
  };

  const modalActionsStyle = {
    display: 'flex',
    justifyContent: 'center',
  };

  const closeButtonStyle = {
    backgroundColor: '#F46C63',
    color: 'black',
    display: 'block',
    boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
    border: 'none',
    borderRadius: '20px',
    padding: '0px',
    fontSize: '1rem',
    cursor: 'pointer',
    width: '100%',
    fontWeight: 'bold',
    paddingTop: '0.5rem',
    paddingBottom: '0.3rem',
  };

  const closeButtonHoverStyle = {
    backgroundColor: '#c3564f',
  };

  return (
    <div
      style={modalOverlayStyle}
      onClick={onClose}
    >
      <div
        style={modalContentStyle}
        onClick={(e) => e.stopPropagation()}
      >
        <div style={modalHeaderStyle}>
          <h3>{title}</h3>
        </div>
        <div style={modalBodyStyle}>
          <p>{errorMessage}</p>
        </div>
        <div style={modalActionsStyle}>
          <button
            style={closeButtonStyle}
            onClick={onClose}
            onMouseEnter={(e) => e.target.style.backgroundColor = closeButtonHoverStyle.backgroundColor}
            onMouseLeave={(e) => e.target.style.backgroundColor = closeButtonStyle.backgroundColor}
          >
            Zamknij
          </button>
        </div>
      </div>
    </div>
  );
};

export default ErrorModal;
