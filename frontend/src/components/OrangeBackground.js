import { Box } from '@mui/material';

export default function OrangeBackground({ children, width, maxHeight, minHeight,height ,overflow, position }) {
  return (
    <Box
      sx={{
        backgroundColor: '#FFE3B3',
        borderRadius: '20px',
        padding: '20px',
        boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
        marginLeft: 'auto',
        marginRight: 'auto',
        gap: '10px',
        alignItems: 'center',
        width: width,
        height: height,
        maxHeight: maxHeight,
        minHeight: minHeight,
        overflow: overflow,
        position:position,
      }}
    >
      {children}
    </Box>
  );
}
