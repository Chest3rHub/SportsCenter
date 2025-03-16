import { Box } from '@mui/material';

export default function GreenBackground({ children, height, marginTop,gap, }) {
  return (
    <Box
      sx={{
        backgroundColor: '#ebf9ea',
        borderRadius: '20px',
        boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
        width: '60%',
        marginLeft: 'auto',
        marginRight: 'auto',
        paddingTop: '7vh',
        paddingBottom: '3vh',
        minHeight: height,
        marginTop: marginTop,
      }}
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          flexGrow: 1,
          rowGap: gap,
          columnGap: gap,
        }}
      >
        {children}
      </Box>
    </Box>
  );
}
