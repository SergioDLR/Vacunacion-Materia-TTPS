import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import CustomButton from "../utils/CustomButtom";
const NavBar = () => {
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "#37BBED", boxShadow: "none" }}>
        <Toolbar>
          <Link to={"/login"} style={{ textDecoration: "none" }}>
            <CustomButton variant="contained" color="success" textColor="white">
              Iniciar sesión
            </CustomButton>
          </Link>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </>
  );
};

export default NavBar;
