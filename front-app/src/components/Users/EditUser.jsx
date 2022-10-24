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
import CustomButton from "../utils/CustomButtom";
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

const EditUser = ({ open, handleClose, getUsers, userSelected }) => {
  const alert = useAlert();
  const [password, setPassword] = useState(userSelected.password);
  const [idJurisdiccion, setIdJurisdiccion] = useState(0);
  const [idRol, setIdRol] = useState(0);
  const { userSesion } = useContext(UserContext);

  const [jurisdicciones, setJurisdicciones] = useState([]);
  const [roles, setRoles] = useState([]);

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
    if (idJurisdiccion === 0) return alert.show("Completa el campo de jurisdiccion");
    if (idRol === 0) return alert.show("Completa el campo de rol");
    axios
      .put(`${allUrls.user}ModificarUsuario`, {
        EmailAdministrador: userSesion.email,
        Email: userSelected.email,
        PasswordNuevo: password,
        IdJurisdiccionNuevo: idJurisdiccion,
        IdRolNuevo: idRol,
      })
      .then((response) => {
        if (response.data.estadoTransaccion === "Aceptada") {
          handleClose();
          alert.show("Modificado correctamente");
          getUsers();
        } else {
          if (response.data.existenciaErrores) alert.show(response.data.errores);
          else alert.show("Ocurrio un error, intentolo mas tarde");
        }
      })
      .catch((error) => alert.show(`${error}Ocurrio un error, intentolo mas tarde`));
  };
  useEffect(() => {
    try {
      axios.get(`${allUrls.roles}GetAll`).then((response) => setRoles(response.data));
      axios.get(`${allUrls.jurisdiccion}GetAll`).then((response) => setJurisdicciones(response.data));
    } catch (e) {
      alert.error(`Ocurrio un error: ${e}`);
    }
  }, []);

  useEffect(() => {
    setPassword(userSelected.password);
    setIdJurisdiccion(userSelected.idJurisdiccion);
    setIdRol(userSelected.idRol);
  }, [open, userSelected]);

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
            Editar un usuario
          </Typography>
          <form onSubmit={handleSubmit}>
            <Typography id="modal-modal-title" variant="h6" component="h2">
              Editando: {userSelected.email}
            </Typography>
            <TextField
              sx={{ display: "block", marginTop: 1 }}
              fullWidth
              id="password-field"
              label="Nueva contraseña"
              variant="filled"
              type="password"
              autoComplete="current-password"
              required
              value={password}
              onChange={handleChangePassword}
            />
            <p>
              <strong> Jurisdiccion anterior: {userSelected.descripcionJurisdiccion} </strong>
            </p>
            <FormControl fullWidth sx={{ marginTop: 1 }}>
              <InputLabel id="input-jurisdiccion">Nueva Jurisdiccion:</InputLabel>
              <Select
                labelId="input-jurisdiccion"
                id="input-jurisdiccion-select"
                onChange={handleChangeJurisdiccion}
                value={idJurisdiccion}
                label="Nueva jurisdiccion"
                defaultValue={1}
              >
                <MenuItem value={0} disabled>
                  Selecciona una opcion
                </MenuItem>
                {jurisdicciones.map((element, index) => (
                  <MenuItem value={element.id} key={index}>
                    {element.descripcion}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <p>
              <strong> Rol anterior: {userSelected.descripcionRol} </strong>
            </p>
            <FormControl fullWidth sx={{ marginTop: 1 }}>
              <InputLabel id="input-rol">Nuevo Rol:</InputLabel>
              <Select
                defaultValue={1}
                labelId="input-rol"
                id="input-jurisdiccion-select"
                onChange={handleChangeRol}
                value={idRol}
                label="Nuevo rol"
              >
                <MenuItem value={0} disabled>
                  Selecciona una opcion
                </MenuItem>
                {roles.map((element, index) => (
                  <MenuItem value={element.id} key={index}>
                    {element.descripcion}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <CustomButton
              sx={{ marginTop: 1, backgroundColor: "red", ":hover": { backgroundColor: "#a1032d" }, marginTop: 1 }}
              variant="contained"
              color={"error"}
              onClick={handleClose}
            >
              Cancelar
            </CustomButton>
            <CustomButton
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
              Modificar
            </CustomButton>
          </form>
        </Box>
      </Modal>
    </div>
  );
};
export default EditUser;
