import { useState, useContext, useEffect } from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import axios from "axios";
import allUrls from "../../services/backend_url";
import { UserContext } from "../Context/UserContext";
import { useAlert } from "react-alert";
import { TextField, FormControl, MenuItem, InputLabel, Select } from "@mui/material";
const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const AddUSer = ({ open, handleClose, getUsers }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const alert = useAlert();
  const [idJurisdiccion, setIdJurisdiccion] = useState(-1);
  const [idRol, setIdRol] = useState(-1);
  const { userSesion } = useContext(UserContext);

  const [jurisdicciones, setJurisdicciones] = useState([]);
  const [roles, setRoles] = useState([]);

  const handleChangeMail = (e) => {
    setEmail(e.target.value);
  };

  const handleChangePassword = (e) => {
    setPassword(e.target.value);
  };
  const handleChangeJurisdiccion = (e) => {
    setIdJurisdiccion(e.target.value);
  };
  const handleChangeRol = (e) => {
    setIdRol(e.target.value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (idJurisdiccion === -1) return alert.show("Completa el campo de jurisdiccion");
    if (idRol === -1) return alert.show("Completa el campo de rol");
    axios
      .post(`${allUrls.user}CrearUsuario`, {
        EmailAdministrador: userSesion.email,
        Email: email,
        Password: password,
        IdJurisdiccion: idJurisdiccion,
        IdRol: idRol,
      })
      .then((response) => {
        if (response.data.estadoTransaccion === "Aceptada") {
          handleClose();
          alert.show("Creado correctamente");
          getUsers();
        } else {
          if (response.data.existenciaErrores) alert.show(response.data.errores);
          else alert.show("Ocurrio un error, intentolo mas tarde");
        }
      })
      .catch((error) => alert.error(`Ocurrio el error: ${error}`));
  };
  useEffect(() => {
    try {
      axios.get(`${allUrls.roles}GetAll`).then((response) => setRoles(response.data));
      axios.get(`${allUrls.jurisdiccion}GetAll`).then((response) => setJurisdicciones(response.data));
    } catch (e) {
      alert.error(`Ocurrio un error: ${e}`);
    }
  }, []);
  return (
    <div>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
        disableEnforceFocus={true}
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Crear nuevo usuario
          </Typography>
          <form onSubmit={handleSubmit}>
            <TextField
              sx={{ display: "block", marginTop: 1 }}
              id="email-field"
              fullWidth
              label="Direccion de correo"
              variant="filled"
              type="email"
              required
              onChange={handleChangeMail}
            />
            <TextField
              sx={{ display: "block", marginTop: 1 }}
              fullWidth
              id="password-field"
              label="Contraseña"
              variant="filled"
              type="password"
              required
              onChange={handleChangePassword}
            />
            <FormControl fullWidth sx={{ marginTop: 1 }}>
              <InputLabel id="input-jurisdiccion">Jurisdiccion</InputLabel>
              <Select
                labelId="input-jurisdiccion"
                id="input-jurisdiccion-select"
                onChange={handleChangeJurisdiccion}
                value={idJurisdiccion}
                label="Jurisdiccion"
                defaultValue={1}
              >
                {jurisdicciones.map((element, index) => (
                  <MenuItem value={element.id} key={index}>
                    {element.descripcion}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <FormControl fullWidth sx={{ marginTop: 1 }}>
              <InputLabel id="input-rol">Rol</InputLabel>
              <Select
                defaultValue={1}
                labelId="input-rol"
                id="input-jurisdiccion-select"
                onChange={handleChangeRol}
                value={idRol}
                label="Rol"
              >
                {roles.map((element, index) => (
                  <MenuItem value={element.id} key={index}>
                    {element.descripcion}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <Button
              sx={{ marginTop: 1, backgroundColor: "red", ":hover": { backgroundColor: "#a1032d" }, marginTop: 1 }}
              variant="contained"
              onClick={handleClose}
            >
              Cancelar
            </Button>
            <Button
              type="submit"
              sx={{
                marginTop: 1,
                backgroundColor: "green",
                ":hover": { backgroundColor: "#387335" },
                marginTop: 1,
                marginLeft: 1,
              }}
              variant="contained"
            >
              Registrar
            </Button>
          </form>
        </Box>
      </Modal>
    </div>
  );
};
export default AddUSer;
