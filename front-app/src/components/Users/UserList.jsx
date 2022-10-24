import User from "./User";
import { useState } from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import EditUser from "./EditUser";
const UserList = ({ users = [], getUsers }) => {
  const [userSelected, setUserSelected] = useState({});
  const [open, setOpen] = useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = (event, reason) => {
    if (reason !== "backdropClick") {
      setOpen(false);
    }
  };
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }}>Nombre</TableCell>
            <TableCell align="right" sx={{ color: "white", fontWeight: 600 }}>
              Jurisdiccion
            </TableCell>
            <TableCell align="right" sx={{ color: "white", fontWeight: 600 }}>
              Rol
            </TableCell>
            <TableCell align="right" sx={{ color: "white", fontWeight: 600 }}>
              Acciones
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {users.map((element, index) => (
            <User user={element} key={index} setUserSelected={setUserSelected} handleOpen={handleOpen} />
          ))}
        </TableBody>
      </Table>
      <EditUser userSelected={userSelected} getUsers={getUsers} open={open} handleClose={handleClose} />
    </TableContainer>
  );
};

export default UserList;
