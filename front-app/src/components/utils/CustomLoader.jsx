import { Backdrop, CircularProgress } from "@mui/material";

const CustomLoader = () => {
  return (
    <Backdrop sx={{ color: "#fff" }} open={true}>
      <CircularProgress color="inherit" />
    </Backdrop>
  );
};

export default CustomLoader;
