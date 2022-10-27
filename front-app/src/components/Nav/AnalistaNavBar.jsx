import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import { UserContext } from "../Context/UserContext";
import CustomButton from "../utils/CustomButtom";
const AnalistaNavBar = () => {
  const { cerrarSesion } = React.useContext(UserContext);
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none" }}>
        <Toolbar>
          <Link to={"/analista/vacunados"} style={{ textDecoration: "none" }}>
            <CustomButton variant="contained" color="info" textColor="white">
              Vacunados
            </CustomButton>
          </Link>
          <Link to={"/analista/vacunas"} style={{ textDecoration: "none" }}>
            <CustomButton variant="contained" color="info" textColor="white">
              stock
            </CustomButton>
          </Link>

          <CustomButton
            textColor="white"
            sx={{ marginLeft: "auto" }}
            color="error"
            variant="contained"
            onClick={cerrarSesion}
          >
            Cerrar sesion
          </CustomButton>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </>
  );
};

export default AnalistaNavBar;
