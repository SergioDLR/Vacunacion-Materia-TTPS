import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import CssBaseline from "@mui/material/CssBaseline";
import { Button, Drawer } from "@mui/material";
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

      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none", overflow: "auto" }}>
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
            <CustomButton textColor="white" color="info" variant="contained" sx={{ minWidth: "max-content" }}>
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
            <CustomButton textColor="white" color="info" variant="contained" sx={{ minWidth: "max-content" }}>
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
            <CustomButton textColor="white" color="info" variant="contained" sx={{ minWidth: "max-content" }}>
              Marcas comerciales
            </CustomButton>
          </NavLink>
          <NavLink
            to={"/operador/vacunados"}
            style={({ isActive }) =>
              isActive
                ? activeStyle
                : {
                    textDecoration: "none",
                    marginRight: 2,
                  }
            }
          >
            <CustomButton textColor="white" color="info" variant="contained" sx={{ minWidth: "max-content" }}>
              Vacunados
            </CustomButton>
          </NavLink>
          <NavLink
            to={"/operador/compras"}
            style={({ isActive }) =>
              isActive
                ? activeStyle
                : {
                    textDecoration: "none",
                    marginRight: 2,
                  }
            }
          >
            <CustomButton textColor="white" color="info" variant="contained" sx={{ minWidth: "max-content" }}>
              Compras
            </CustomButton>
          </NavLink>
          <NavLink
            to={"/operador/distribuciones"}
            style={({ isActive }) =>
              isActive
                ? activeStyle
                : {
                    textDecoration: "none",
                    marginRight: 2,
                  }
            }
          >
            <CustomButton textColor="white" color="info" variant="contained" sx={{ minWidth: "max-content" }}>
              Distribuciones
            </CustomButton>
          </NavLink>
          <NavLink
            to={"/operador/stock"}
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
              Stock
            </CustomButton>
          </NavLink>
          <CustomButton
            textColor="white"
            sx={{ marginLeft: "auto", minWidth: "max-content" }}
            color="error"
            variant="contained"
            onClick={cerrarSesion}
          >
            Cerrar sesión
          </CustomButton>
        </Toolbar>
      </AppBar>

      <Toolbar />
    </>
  );
};

export default OperadorNavBar;
