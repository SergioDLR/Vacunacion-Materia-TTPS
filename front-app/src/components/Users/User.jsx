import TableRow from "@mui/material/TableRow";
import TableCell from "@mui/material/TableCell";
import UserActions from "./UserActions";
const User = ({ user = {}, setUserSelected, handleOpen }) => {
  const handleSelected = () => {
    handleOpen();
    setUserSelected(user);
  };
  return (
    <TableRow scope="row">
      <TableCell>{user.email}</TableCell>
      <TableCell align="right">{user.descripcionJurisdiccion}</TableCell>
      <TableCell align="right">{user.descripcionRol}</TableCell>
      <UserActions handleSelected={handleSelected} />
    </TableRow>
  );
};

export default User;
