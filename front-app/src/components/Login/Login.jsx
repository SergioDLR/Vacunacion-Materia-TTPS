import { TextField, Box, Paper, Container, Button } from "@mui/material";
import { useState, useContext } from "react";
import { UserContext } from "../Context/UserContext";
import NavBar from "../Nav/NavBar";
import { useAlert } from "react-alert";
import allUrls from "../../services/backend_url";
import axios from "axios";
const Login = () => {
  const alert = useAlert();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { logIn } = useContext(UserContext);
  const handleChangeMail = (e) => {
    setEmail(e.target.value);
  };

  const handleChangePassword = (e) => {
    setPassword(e.target.value);
  };
  const handleSubmit = (e) => {
    e.preventDefault();
    if (email.length < 5) return alert("La direccion de correo es muy corta");
    if (password.length < 3) return alert("Completa el campo de la Contraseña");
    try {
      axios
        .get(`${allUrls.user}Login?email=${email}&password=${password}`)
        .then((response) => {
          if (response.data.estadoTransaccion === "Rechazada") {
            alert.error(response.data.errores);
          } else {
            logIn(response.data.usuarioDTO);
          }
        })
        .catch((e) => alert.error(`Ocurrio el error ${e}`));
    } catch (e) {
      alert.error(`Ocurrio el error ${e}`);
    }
  };
  return (
    <>
      <NavBar />
      <Container maxWidth="sm">
        <Box sx={{ marginTop: 1 }}>
          <Paper sx={{ padding: 3 }}>
            <form onSubmit={handleSubmit}>
              <TextField
                sx={{ display: "block" }}
                id="email-field"
                fullWidth
                label="Direccion de correo"
                variant="filled"
                type="email"
                required
                onChange={handleChangeMail}
              />
              <TextField
                sx={{ display: "block" }}
                fullWidth
                id="password-field"
                label="Contraseña"
                variant="filled"
                type="password"
                required
                onChange={handleChangePassword}
              />
              <Button type="submit" sx={{ marginTop: 1 }} variant="contained">
                Entrar
              </Button>
            </form>
          </Paper>
        </Box>
      </Container>
    </>
  );
};

export default Login;
