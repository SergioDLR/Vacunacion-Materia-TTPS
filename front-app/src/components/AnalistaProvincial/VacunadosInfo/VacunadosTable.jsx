import { TableContainer, TableRow, TableCell, TableBody, Table, Paper, TableHead } from "@mui/material";
import Vacunado from "./Vacunado";
const VacunadosTable = ({ vacunados }) => {
  return (
    <TableContainer component={Paper} sx={{ marginTop: 1 }}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }}>DNI</TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Nombre
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Apellido
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Fecha vacunacion
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Jurisdiccion
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Vacuna
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Marca comercial
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Dosis
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {vacunados.map((element, index) => (
            <Vacunado vacunado={element} key={index} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default VacunadosTable;
