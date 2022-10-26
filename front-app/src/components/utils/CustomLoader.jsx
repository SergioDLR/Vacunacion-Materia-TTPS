import { Backdrop, CircularProgress } from "@mui/material";

const CustomLoader = () => {
  return (
    <Backdrop sx={{ color: "#fff", zIndex: 99999999999999 }} open={true}>
      <CircularProgress color="inherit" />
    </Backdrop>
  );
};

export default CustomLoader;
