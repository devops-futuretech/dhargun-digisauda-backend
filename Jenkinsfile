pipeline {
    agent any

    environment {
        // DOTNET_VERSION = '4.7.1'
        SCANNER_HOME = tool name: 'sonar-scanner-msbuild-fm', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
        MSBUILD_HOME = 'C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin'
        NUGET_HOME = 'C:\\tools\\nuget'
        CURRENT_DIR = pwd()
    }

    stages {

        stage('Get Commit Message') {
            steps {
                script {
                    env.GIT_COMMIT_MSG = bat(script: 'git log -1 --pretty=format:%%B', returnStdout: true).trim()
                }
                echo "Commit Message: ${env.GIT_COMMIT_MSG}"
            }
        }

        stage('Delete Package'){
            steps {
                    bat "rmdir /S /Q ${CURRENT_DIR}\\AdaniWilmar.Solution\\packages"
            }
        }

        stage('Restore Packages') {
            steps {
                dir("${CURRENT_DIR}\\AdaniWilmar.Solution") {
                    bat "\"${NUGET_HOME}\\nuget.exe\" restore AdaniWilmar.Solution.sln"
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                dir("${CURRENT_DIR}\\AdaniWilmar.Solution") {
                    withCredentials([
                        string(credentialsId: 'sonar_token', variable: 'SONAR_TOKEN'),
                        string(credentialsId: 'sonar-host-url', variable: 'SONAR_URL')
                    ]) {
                        withSonarQubeEnv('sonar') {
                            bat """
                                \"${SCANNER_HOME}\\SonarScanner.MSBuild.exe\" begin /k:\"AdaniWilmar_DigitalSaudaApp_Web_dev\" /d:sonar.exclusions=\"Dockerfile\" /d:sonar.host.url=\"${SONAR_URL}\" /d:sonar.login=\"${SONAR_TOKEN}\"
                                \"${MSBUILD_HOME}\\MSBuild.exe\" /t:Rebuild
                                \"${SCANNER_HOME}\\SonarScanner.MSBuild.exe\" end /d:sonar.login=\"${SONAR_TOKEN}\"
                            """
                        }
                    }
                }
            }
        }

        stage('Sleep after SonarQube Analysis') {
            steps {
                sleep(time: 30, unit: 'SECONDS')
            }
        }

        stage('Quality Gate') {
            steps {
                script {
                    timeout(time: 3, unit: 'MINUTES') {
                        def qg = waitForQualityGate()
                        // if (qg.status != 'OK') {
                        //     error "Pipeline aborted due to quality gate failure: ${qg.status}"
                        // }
                    }
                }
            }
        }

        stage('OWASP Dependency-Check Vulnerabilities') {
            steps {
                dependencyCheck additionalArguments: '''
                    --out './'
                    --scan 'AdaniWilmar.Solution/'
                    --format 'ALL'
                    --nvdApiKey "30b77b00-dfa9-4c8f-a535-f588221e5fb8"
                    --failOnCVSS 7
                    --prettyPrint''', odcInstallation: 'Dependency-Check'
                
                dependencyCheckPublisher pattern: 'dependency-check-report.xml'
            }
        }

        stage('Archive dependency-check Reports') {
            steps {
                archiveArtifacts artifacts: 'dependency-check-report.html', allowEmptyArchive: true
            }
        }
    }

    post {
        success {
            emailext(
                subject: "Success! ${env.JOB_NAME} Build Passed!",
                body: """
                    <html>
                        <body style="font-family: Arial, sans-serif; line-height: 1.6;">
                            <div style="border: 1px solid #d4d4d4; padding: 16px; border-radius: 8px;">
                                <h2 style="color: #28a745; margin-bottom: 0;">Build Succeeded!</h2>
                                <p style="margin-top: 0; color: #555;">Everything went smoothly. Great job!</p>
                                <div style="padding: 10px; border-left: 4px solid #28a745; background-color: #f9f9f9;">
                                    <p><b>Branch:</b> <span style="color: #007bff;">${env.GIT_BRANCH}</span></p>
                                    <p><b>Job Name:</b> ${env.JOB_NAME}</p>
                                    <p><b>Commit Message:</b> <span style="color: #6c757d;">"${env.GIT_COMMIT_MSG}"</span></p>
                                    <p><b>Build Number:</b> #${env.BUILD_NUMBER}</p>
                                    <p><b>Details:</b> <a href="${env.BUILD_URL}" style="color: #007bff;">View Build</a></p>
                                </div>
                            </div>
                        </body>
                    </html>
                """,
                mimeType: 'text/html',
                to: "deepan.sivakumar@impigertech.com"
            )
            cleanWs()
        }
    
        failure {
            emailext(
                subject: "Oops! ${env.JOB_NAME} Build Failed",
                body: """
                    <html>
                        <body style="font-family: Arial, sans-serif; line-height: 1.6;">
                            <div style="border: 1px solid #d4d4d4; padding: 16px; border-radius: 8px;">
                                <h2 style="color: #dc3545; margin-bottom: 0;">Build Failed!</h2>
                                <p style="margin-top: 0; color: #555;">Something went wrong, Take a look into this!</p>
                                <div style="padding: 10px; border-left: 4px solid #dc3545; background-color: #f9f9f9;">
                                    <p><b>Branch:</b> <span style="color: #007bff;">${env.GIT_BRANCH}</span></p>
                                    <p><b>Job Name:</b> ${env.JOB_NAME}</p>
                                    <p><b>Commit Message:</b> <span style="color: #6c757d;">"${env.GIT_COMMIT_MSG}"</span></p>
                                    <p><b>Build Number:</b> #${env.BUILD_NUMBER}</p>
                                    <p><b>Details:</b> <a href="${env.BUILD_URL}" style="color: #007bff;">View Build</a></p>
                                </div>
                            </div>
                        </body>
                    </html>
                """,
                mimeType: 'text/html',
                to: "deepan.sivakumar@impigertech.com"
            )
            cleanWs()
        }
    }
}