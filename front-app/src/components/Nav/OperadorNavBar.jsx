import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link, NavLink } from "react-router-dom";
import { UserContext } from "../Context/UserContext";
const OperadorNavBar = () => {
  const { cerrarSesion } = React.useContext(UserContext);
  let activeStyle = {
    textDecoration: "solid underline white 2px",
    marginRight: 2,
  };
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "black" }}>
        <Toolbar>
          <NavLink
            style={({ isActive }) =>
              isActive
                ? activeStyle
                : {
                    textDecoration: "none",
                    marginRight: 2,
                  }
            }
            to={"/operador/vacunas"}
          >
            <Button variant="contained">Vacunas</Button>
          </NavLink>
          <NavLink
            style={({ isActive }) =>
              isActive
                ? activeStyle
                : {
                    textDecoration: "none",
                    marginRight: 2,
                  }
            }
            to={"/operador/vacunasdesarrolladas"}
          >
            <Button variant="contained">Vacunas desarrolladas</Button>
          </NavLink>
          <NavLink
            to={"/operador/marcascomerciales"}
            style={({ isActive }) =>
              isActive
                ? activeStyle
                : {
                    textDecoration: "none",
                    marginRight: 2,
                  }
            }
          >
            <Button variant="contained">Marcas comerciales</Button>
          </NavLink>
          <Button sx={{ marginLeft: "auto" }} color="error" onClick={cerrarSesion} variant="contained">
            Cerrar sesion
          </Button>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </>
  );
};

export default OperadorNavBar;
