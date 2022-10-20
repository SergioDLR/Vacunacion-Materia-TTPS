import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import CssBaseline from "@mui/material/CssBaseline";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";

const OperadorNavBar = () => {
  return (
    <>
      <CssBaseline />
      <AppBar sx={{ backgroundColor: "black" }}>
        <Toolbar>
          <Link to={"/operador/vacunas"} style={{ textDecoration: "none" }}>
            <Button variant="contained">Vacunas</Button>
          </Link>
        </Toolbar>
      </AppBar>
      <Toolbar />
    </>
  );
};

export default OperadorNavBar;
