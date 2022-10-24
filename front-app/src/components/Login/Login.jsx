import { TextField, Box, Paper, Container, Button, CircularProgress } from "@mui/material";
import { useState, useContext } from "react";
import { UserContext } from "../Context/UserContext";
import NavBar from "../Nav/NavBar";
import { useAlert } from "react-alert";
import allUrls from "../../services/backend_url";
import axios from "axios";
import CustomLoader from "../utils/CustomLoader";
import CustomButton from "../utils/CustomButtom";
const Login = () => {
  const alert = useAlert();
  const [email, setEmail] = useState("");
  const [estaCargando, setEstaCargando] = useState(false);
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
    setEstaCargando(true);
    try {
      axios
        .get(`${allUrls.user}Login?email=${email}&password=${password}`)
        .then((response) => {
          if (response.data.estadoTransaccion === "Rechazada") {
            alert.error(response.data.errores);
          } else {
            logIn(response.data.usuarioDTO);
            alert.success("Logeado correctamente");
          }
        })
        .catch((e) => alert.error(`Ocurrio el error ${e}`))
        .finally(() => {
          setEstaCargando(false);
        });
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
            <h3 style={{ margin: 0, marginBottom: 1 }}>Ingresar</h3>
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
              <CustomButton color={"success"} type="submit" sx={{ marginTop: 1 }} variant="contained">
                Entrar
              </CustomButton>
            </form>
          </Paper>
          {estaCargando && <CustomLoader />}
        </Box>
      </Container>
    </>
  );
};

export default Login;
