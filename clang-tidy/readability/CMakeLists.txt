set(LLVM_LINK_COMPONENTS support)

add_clang_library(clangTidyReadabilityModule
  AvoidConstParamsInDecls.cpp
  BracesAroundStatementsCheck.cpp
  ContainerSizeEmptyCheck.cpp
  DeleteNullPointerCheck.cpp
  DeletedDefaultCheck.cpp
  ElseAfterReturnCheck.cpp
  FunctionSizeCheck.cpp
  IdentifierNamingCheck.cpp
  ImplicitBoolCastCheck.cpp
  InconsistentDeclarationParameterNameCheck.cpp
  MisleadingIndentationCheck.cpp
  MisplacedArrayIndexCheck.cpp
  NamedParameterCheck.cpp
  NamespaceCommentCheck.cpp
  NonConstParameterCheck.cpp
  ReadabilityTidyModule.cpp
  RedundantControlFlowCheck.cpp
  RedundantDeclarationCheck.cpp
  RedundantFunctionPtrDereferenceCheck.cpp
  RedundantMemberInitCheck.cpp
  RedundantStringCStrCheck.cpp
  RedundantSmartptrGetCheck.cpp
  RedundantStringInitCheck.cpp
  SimplifyBooleanExprCheck.cpp
  StaticDefinitionInAnonymousNamespaceCheck.cpp
  UniqueptrDeleteReleaseCheck.cpp

  LINK_LIBS
  clangAST
  clangASTMatchers
  clangBasic
  clangLex
  clangTidy
  clangTidyUtils
  clangTooling
  )
