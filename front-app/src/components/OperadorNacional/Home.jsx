import { Box } from "@mui/material";
const HomeOperador = () => {
  return (
    <Box
      sx={{
        backgroundImage:
          "url(https://www.argentina.gob.ar/sites/default/files/styles/jumbotron/public/2020-headers_web-1920x449-covid_0.jpg)",
        width: "100%",
        height: "200px",
        backgroundSize: "cover",
        backgroundPosition: "center center",
        position: "relative",
      }}
    >
      <Box sx={{ height: "100%", width: "100%", position: "absolute", background: "rgba(0,0,0,.5)" }}></Box>
      <Box sx={{ position: "absolute", color: "white", top: "10%", left: "40%", transformOrigin: "center" }}>
        <h2 style={{ textAlign: "center" }}>Bienvenido operador</h2>
      </Box>
    </Box>
  );
};

export default HomeOperador;
