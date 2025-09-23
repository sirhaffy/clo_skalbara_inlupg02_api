FROM public.ecr.aws/lambda/dotnet:9-x86_64

# Copy function code
COPY bin/Release/net9.0/linux-x64/publish/ ${LAMBDA_TASK_ROOT}

# Set the CMD to your handler
CMD ["ApiLambda"]