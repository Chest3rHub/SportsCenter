import { Box } from '@mui/material';

export default function Header({ children, marginTop }) {
  return (
    <Box
      sx={{
        backgroundColor: '#AFEBBC',
        boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
        width: '75%',
        borderRadius: '100px',
        padding: '10px 0',
        fontSize: '2rem',
        color: '#000000',
        textAlign: 'center',
        margin: 'auto',
        fontWeight: 'bold',
        marginBottom: '30px',
        marginTop: marginTop,
      }}
    >
      {children}
    </Box>
  );
}
