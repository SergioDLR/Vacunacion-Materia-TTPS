import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import CssBaseline from "@mui/material/CssBaseline";
import CustomButton from "../utils/CustomButtom";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import { UserContext } from "../Context/UserContext";

const AdminNavBar = () => {
  const { cerrarSesion } = React.useContext(UserContext);
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none" }}>
        <Toolbar>
          <Link to={"/admin/users"} style={{ textDecoration: "none" }}>
            <CustomButton textColor="white" color="info" variant="contained">
              Usuarios
            </CustomButton>
          </Link>
          <CustomButton
            textColor="white"
            sx={{ marginLeft: "auto" }}
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

export default AdminNavBar;
