import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { NavLink } from "react-router-dom";
import { UserContext } from "../Context/UserContext";
import CustomButton from "../utils/CustomButtom";
const OperadorNavBar = () => {
  const { cerrarSesion } = React.useContext(UserContext);
  let activeStyle = {
    textDecoration: "solid underline white 2px",
    marginRight: 2,
  };
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none" }}>
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
            <CustomButton textColor="white" color="info" variant="contained">
              Vacunas
            </CustomButton>
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
            <CustomButton textColor="white" color="info" variant="contained">
              Vacunas desarrolladas
            </CustomButton>
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
            <CustomButton textColor="white" color="info" variant="contained">
              Marcas comerciales
            </CustomButton>
          </NavLink>
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

export default OperadorNavBar;
