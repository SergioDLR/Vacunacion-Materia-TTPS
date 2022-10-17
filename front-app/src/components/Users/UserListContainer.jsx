import { useEffect, useContext, useState } from "react";
import { UserContext } from "../Context/UserContext";
import allUrls from "../../services/backend_url";
import axios from "axios";
import UserList from "./UserList";
import { Container } from "@mui/system";
import { Button, Box } from "@mui/material";
import CircularProgress from "@mui/material/CircularProgress";
import { useAlert } from "react-alert";
import AddUSer from "./AddUser";
const UserListContainer = () => {
  const alert = useAlert();
  const { userSesion } = useContext(UserContext);
  const [open, setOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  const handleOpen = () => setOpen(true);
  const handleClose = (event, reason) => {
    if (reason !== "backdropClick") {
      setOpen(false);
    }
  };
  const [users, setUsers] = useState([]);
  useEffect(() => {
    getUsers();
  }, []);

  const getUsers = () => {
    try {
      axios
        .get(`${allUrls.user}GetAll?emailAdministrador=${userSesion.email}`)
        .then((response) => {
          if (response?.data.estadoTransaccion === "Aceptada") {
            setUsers(response?.data.listaUsuariosDTO);
          } else {
            alert.error(response?.data.errores);
          }
          setIsLoading(false);
        })
        .catch((e) => console.log(e));
    } catch (e) {
      alert.error(`Ocurrio un error indeterminado ${e}`);
    }
  };

  return (
    <Container>
      {!isLoading ? (
        <>
          <Button
            variant="contained"
            sx={{ display: "table", margin: "auto", marginTop: 1, marginBottom: 1 }}
            onClick={handleOpen}
          >
            Agregar usuario
          </Button>
          <UserList users={users} getUsers={getUsers} />
          <AddUSer open={open} handleClose={handleClose} getUsers={getUsers} />
        </>
      ) : (
        <Box sx={{ marginTop: 1 }}>
          <CircularProgress sx={{ display: "table", margin: "auto" }} />
        </Box>
      )}
    </Container>
  );
};

export default UserListContainer;
