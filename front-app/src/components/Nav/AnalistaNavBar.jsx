import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import { UserContext } from "../Context/UserContext";

const AnalistaNavBar = () => {
  const { cerrarSesion } = React.useContext(UserContext);
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none" }}>
        <Toolbar>
          <Link to={"/analista/vacunados"} style={{ textDecoration: "none" }}>
            <Button variant="contained">Vacunados</Button>
          </Link>
          <Link to={"/analista/vacunas"} style={{ textDecoration: "none" }}>
            <Button variant="contained">Vacunas</Button>
          </Link>

          <Button sx={{ backgroundColor: "red", marginLeft: 1 }} onClick={cerrarSesion} variant="contained">
            Cerrar sesion
          </Button>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </>
  );
};

export default AnalistaNavBar;
