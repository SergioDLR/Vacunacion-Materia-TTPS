import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";

const NavBar = () => {
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "black" }}>
        <Toolbar>
          <Link to={"login"}>
            <Button variant="contained">Iniciar sesion</Button>
          </Link>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </>
  );
};

export default NavBar;
