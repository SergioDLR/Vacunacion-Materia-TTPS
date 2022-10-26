import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import { UserContext } from "../Context/UserContext";
import CustomButton from "../utils/CustomButtom";
const VacunadorNavBar = () => {
  const { cerrarSesion } = React.useContext(UserContext);
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none" }}>
        <Toolbar>
          <Link style={{ textDecoration: "none" }} to="/vacunador/aplicar">
            <CustomButton textColor="white" color="info" variant="contained">
              Aplicar vacuna
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

export default VacunadorNavBar;
